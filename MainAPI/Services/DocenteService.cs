using MainAPI.Data;
using MainAPI.Models;
using MainAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public class DocenteService : IDocenteService
    {
        private readonly MainDbContext _context;

        public DocenteService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetDocenteIdAsync(int loginUserId)
        {
            var perfil = await _context.PerfilCatedraticos.FirstOrDefaultAsync(p => p.IdPersonaNavigation.LoginUserId == loginUserId);
            return perfil?.IdCatedratico ?? 0;
        }

        public async Task<object?> GetMisCursosAsync(int idCatedratico)
        {
            var cursosDb = await (from ch in _context.CursoHabilitados
                                join sec in _context.Seccions on ch.IdSeccion equals sec.IdSeccion
                                join csc in _context.CarreraSemestreCursos on ch.IdCarreraSemestreCurso equals csc.IdCarreraSemestreCurso
                                join cs in _context.CarreraSemestres on csc.IdCarreraSemestre equals cs.IdCarreraSemestre
                                join sem in _context.Semestres on cs.IdSemestre equals sem.IdSemestre
                                join c in _context.Cursos on csc.IdCurso equals c.IdCurso
                                join car in _context.Carreras on cs.IdCarrera equals car.IdCarrera
                                join fac in _context.Facultads on car.IdFacultad equals fac.IdFacultad
                                join ciclo in _context.CicloEscolars on ch.IdCiclo equals ciclo.IdCiclo
                                where ch.IdCatedratico == idCatedratico
                                select new
                                {
                                    IdCursoHabilitado = ch.IdCursoHabilitado,
                                    NombreCurso = c.NombreCurso,
                                    Seccion = sec.NombreSeccion,
                                    Carrera = car.NombreCarrera,
                                    Facultad = fac.NombreFacultad,
                                    Semestre = sem.NombreSemestre,
                                    HorarioInicio = ch.HorarioInicio,
                                    HorarioFin = ch.HorarioFin,
                                    Estado = ch.Estado,
                                    EsHistorico = (ch.Estado != "activo" || ciclo.Estado != true)
                                }).ToListAsync();

            var idsCursos = cursosDb.Select(c => c.IdCursoHabilitado).ToList();
            var horariosDb = new List<HorarioCurso>();
            
            if (idsCursos.Any())
            {
                horariosDb = await _context.HorarioCursos
                    .Where(hc => idsCursos.Contains(hc.IdCursoHabilitado))
                    .ToListAsync();
            }

            return cursosDb.Select(c => 
            {
                var dias = horariosDb.Where(h => h.IdCursoHabilitado == c.IdCursoHabilitado).Select(h => h.DiaSemana).ToList();
                return new
                {
                    c.IdCursoHabilitado,
                    c.NombreCurso,
                    c.Seccion,
                    c.Carrera,
                    c.Facultad,
                    c.Semestre,
                    Horario = $"{string.Join(", ", dias)} | {c.HorarioInicio?.ToString("HH:mm") ?? "N/A"} - {c.HorarioFin?.ToString("HH:mm") ?? "N/A"}",
                    c.Estado,
                    c.EsHistorico
                };
            }).ToList();
        }

        public async Task<object?> GetSemanasCursoAsync(int idCursoHabilitado)
        {
            var cursoHab = await _context.CursoHabilitados
                .Include(c => c.IdCicloNavigation)
                .FirstOrDefaultAsync(c => c.IdCursoHabilitado == idCursoHabilitado);

            if (cursoHab == null) return null;

            var fechaInicioCiclo = cursoHab.IdCicloNavigation.FechaInicio ?? DateOnly.FromDateTime(DateTime.Now);
            var fechaFinCiclo = cursoHab.IdCicloNavigation.FechaFinalizacion ?? DateOnly.FromDateTime(DateTime.Now.AddMonths(6));

            var materiales = await _context.MaterialClases.Where(m => m.IdCursoHabilitado == idCursoHabilitado).ToListAsync();
            var tareas = await _context.Tareas.Where(t => t.IdCursoHabilitado == idCursoHabilitado).ToListAsync();

            var semanas = new List<object>();
            var currentDate = fechaInicioCiclo.ToDateTime(TimeOnly.MinValue);
            var realNow = DateTime.Now;

            int i = 1;
            while (currentDate <= fechaFinCiclo.ToDateTime(TimeOnly.MinValue))
            {
                var endOfWeek = currentDate.AddDays(6);
                var isCurrent = (realNow >= currentDate && realNow <= endOfWeek.AddDays(1).AddSeconds(-1));

                var materialesSemana = materiales.Where(m => m.FechaSubida.HasValue &&
                    m.FechaSubida.Value.ToDateTime(TimeOnly.MinValue) >= currentDate &&
                    m.FechaSubida.Value.ToDateTime(TimeOnly.MinValue) <= endOfWeek).ToList();

                var tareasSemana = tareas.Where(t => t.FechaCreacion.HasValue &&
                    t.FechaCreacion.Value.ToDateTime(TimeOnly.MinValue) >= currentDate &&
                    t.FechaCreacion.Value.ToDateTime(TimeOnly.MinValue) <= endOfWeek).ToList();

                semanas.Add(new
                {
                    NumeroSemana = i,
                    Titulo = $"{currentDate:dd MMM} - {endOfWeek:dd MMM}",
                    EsSemanaActual = isCurrent,
                    FechaInicioSemana = currentDate,
                    Materiales = materialesSemana.Select(m => new { m.IdMaterial, m.Titulo, m.UrlDocumento }),
                    Tareas = tareasSemana.Select(t => new { t.IdTarea, t.Titulo, t.FechaVencimiento, t.PunteoMaximo })
                });

                currentDate = currentDate.AddDays(7);
                i++;
            }

            return semanas;
        }

        public async Task<(bool IsSuccess, string Message, object? Tarea)> CrearTareaAsync(int idCursoHabilitado, CrearTareaDto dto)
        {
            var cursoObj = await (from ch in _context.CursoHabilitados
                                  join csc in _context.CarreraSemestreCursos on ch.IdCarreraSemestreCurso equals csc.IdCarreraSemestreCurso
                                  join c in _context.Cursos on csc.IdCurso equals c.IdCurso
                                  where ch.IdCursoHabilitado == idCursoHabilitado
                                  select c).FirstOrDefaultAsync();

            var tope = cursoObj?.PunteoMaximoTotal ?? 100.00m;

            var puntosActualesTareas = await _context.Tareas
                .Where(t => t.IdCursoHabilitado == idCursoHabilitado)
                .SumAsync(t => t.PunteoMaximo);

            var puntosParciales = await _context.EvaluacionFijas
                .Where(e => e.IdCursoHabilitado == idCursoHabilitado)
                .SumAsync(e => e.PunteoAsignado);

            if ((puntosActualesTareas + puntosParciales + dto.PunteoMaximo) > tope)
            {
                return (false, $"Error: La creación de esta tarea ({dto.PunteoMaximo} pts) excede el máximo del curso ({tope} pts). Puntos ocupados actuales: {puntosActualesTareas + puntosParciales}.", null);
            }

            var nuevaTarea = new Tarea
            {
                IdCursoHabilitado = idCursoHabilitado,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                PunteoMaximo = dto.PunteoMaximo,
                FechaVencimiento = dto.FechaVencimiento,
                Visibilidad = dto.Visibilidad,
                FechaCreacion = dto.FechaAsignacion.HasValue
                    ? DateOnly.FromDateTime(dto.FechaAsignacion.Value)
                    : DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Tareas.Add(nuevaTarea);
            await _context.SaveChangesAsync();
            return (true, "Tarea creada exitosamente", nuevaTarea);
        }

        public async Task<object> CrearMaterialAsync(int idCursoHabilitado, CrearMaterialDto dto)
        {
            var nuevoMaterial = new MaterialClase
            {
                IdCursoHabilitado = idCursoHabilitado,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                UrlDocumento = dto.UrlDocumento,
                Visibilidad = dto.Visibilidad,
                FechaSubida = dto.FechaAsignacion.HasValue
                    ? DateOnly.FromDateTime(dto.FechaAsignacion.Value)
                    : DateOnly.FromDateTime(DateTime.Now)
            };

            _context.MaterialClases.Add(nuevoMaterial);
            await _context.SaveChangesAsync();
            return nuevoMaterial;
        }

        public async Task<object> GetGradebookAsync(int idCursoHabilitado)
        {
            var confZona = await _context.ConfiguracionSistemas.FirstOrDefaultAsync(c => c.Clave == "zona_minima_examen");
            var confAsist = await _context.ConfiguracionSistemas.FirstOrDefaultAsync(c => c.Clave == "asistencia_minima_porcentaje");
            var confAprob = await _context.ConfiguracionSistemas.FirstOrDefaultAsync(c => c.Clave == "nota_minima_aprobacion");

            decimal zonaMinima = confZona != null ? decimal.Parse(confZona.Valor) : 31m;
            decimal asistenciaMinima = confAsist != null ? decimal.Parse(confAsist.Valor) : 80m;
            decimal notaAprobacion = confAprob != null ? decimal.Parse(confAprob.Valor) : 61m;

            var alumnos = await _context.AsignacionCursos
                .Include(a => a.IdEstudianteNavigation.IdPersonaNavigation)
                .Where(a => a.IdCursoHabilitado == idCursoHabilitado)
                .ToListAsync();

            var evaluacionesFijas = await _context.EvaluacionFijas.Where(e => e.IdCursoHabilitado == idCursoHabilitado).ToListAsync();
            var idParcial1 = evaluacionesFijas.FirstOrDefault(e => e.TipoEvaluacion.Contains("Parcial 1"))?.IdEvaluacion ?? 0;
            var idParcial2 = evaluacionesFijas.FirstOrDefault(e => e.TipoEvaluacion.Contains("Parcial 2"))?.IdEvaluacion ?? 0;
            var idParcial3 = evaluacionesFijas.FirstOrDefault(e => e.TipoEvaluacion.Contains("Parcial 3"))?.IdEvaluacion ?? 0;
            var idFinal = evaluacionesFijas.FirstOrDefault(e => e.TipoEvaluacion.Contains("Final"))?.IdEvaluacion ?? 0;

            var tareas = await _context.Tareas.Where(t => t.IdCursoHabilitado == idCursoHabilitado).ToListAsync();
            var sesionesTotales = await _context.ClaseSesions.Where(c => c.IdCursoHabilitado == idCursoHabilitado).ToListAsync();

            var gradebook = new List<object>();

            foreach (var a in alumnos)
            {
                var clasesValidas = sesionesTotales.Where(s => s.FechaSesion >= a.FechaAsignacion).ToList();
                int totalClases = clasesValidas.Count;
                decimal porcentajeAsistencia = 100m; 

                if (totalClases > 0)
                {
                    var idSesionesValidas = clasesValidas.Select(s => s.IdSesion).ToList();
                    var asistencias = await _context.AsistenciaEstudiantes
                        .Where(ast => ast.IdEstudiante == a.IdEstudiante && idSesionesValidas.Contains(ast.IdSesion) && (ast.EstadoAsistencia == "Presente" || ast.EstadoAsistencia == "Excusa" || ast.EstadoAsistencia == "Tarde"))
                        .CountAsync();

                    porcentajeAsistencia = ((decimal)asistencias / totalClases) * 100m;
                }

                var entregasEstudiante = await _context.EntregaTareas.Where(e => e.IdEstudiante == a.IdEstudiante && tareas.Select(t => t.IdTarea).Contains(e.IdTarea)).ToListAsync();
                decimal puntosTareas = entregasEstudiante.Sum(e => e.Calificacion ?? 0m);

                var notasFijas = await _context.CalificacionEvaluacions.Where(c => c.IdEstudiante == a.IdEstudiante).ToListAsync();
                decimal nP1 = notasFijas.FirstOrDefault(n => n.IdEvaluacion == idParcial1)?.NotaObtenida ?? 0m;
                decimal nP2 = notasFijas.FirstOrDefault(n => n.IdEvaluacion == idParcial2)?.NotaObtenida ?? 0m;
                decimal nP3 = notasFijas.FirstOrDefault(n => n.IdEvaluacion == idParcial3)?.NotaObtenida ?? 0m;
                decimal nFinal = notasFijas.FirstOrDefault(n => n.IdEvaluacion == idFinal)?.NotaObtenida ?? 0m;

                decimal zonaAcumulada = puntosTareas + nP1 + nP2 + nP3;
                bool tieneDerechoExamen = (zonaAcumulada >= zonaMinima) && (porcentajeAsistencia >= asistenciaMinima);

                decimal notaTotal = zonaAcumulada + nFinal;
                string estado = notaTotal >= notaAprobacion ? "Aprobado" : "Reprobado";

                if (!tieneDerechoExamen) { notaTotal = zonaAcumulada; estado = "Sin Derecho (SDE)"; }

                gradebook.Add(new
                {
                    IdEstudiante = a.IdEstudiante,
                    NombreCompleto = $"{a.IdEstudianteNavigation.IdPersonaNavigation.PrimerNombre} {a.IdEstudianteNavigation.IdPersonaNavigation.PrimerApellido}",
                    Asistencia = Math.Round(porcentajeAsistencia, 2),
                    Tareas = Math.Round(puntosTareas, 2),
                    Parcial1 = nP1,
                    Parcial2 = nP2,
                    Parcial3 = nP3,
                    ZonaAcumulada = Math.Round(zonaAcumulada, 2),
                    DerechoExamen = tieneDerechoExamen,
                    ExamenFinal = nFinal,
                    NotaTotal = Math.Round(notaTotal, 2),
                    Estado = estado
                });
            }

            return gradebook;
        }

        public async Task<(bool IsSuccess, string Message, decimal? PuntosCalculados)> CalificarPorPorcentajeAsync(int idEntrega, CalificarDto dto)
        {
            var entrega = await _context.EntregaTareas.Include(e => e.IdTareaNavigation).FirstOrDefaultAsync(e => e.IdEntrega == idEntrega);
            if (entrega == null) return (false, "Entrega no encontrada", null);

            entrega.PorcentajeObtenido = dto.PorcentajeObtenido;
            entrega.Calificacion = (entrega.IdTareaNavigation.PunteoMaximo * dto.PorcentajeObtenido) / 100m;
            entrega.ComentariosCatedratico = dto.Comentarios;

            await _context.SaveChangesAsync();
            return (true, "Calificación guardada.", entrega.Calificacion);
        }

        public async Task<(bool IsSuccess, string Message)> CalificarDirectoAsync(int idEntrega, CalificacionDto dto)
        {
            var entrega = await _context.EntregaTareas.FindAsync(idEntrega);
            if (entrega == null) return (false, "Entrega no encontrada.");
            entrega.Calificacion = dto.Calificacion;
            entrega.ComentariosCatedratico = dto.Comentarios;

            await _context.SaveChangesAsync();
            return (true, "Calificación guardada correctamente.");
        }

        public async Task<object> GetParticipantesAsync(int idCursoHabilitado)
        {
            var participantes = await _context.AsignacionCursos
                .Include(a => a.IdEstudianteNavigation.IdPersonaNavigation)
                .Where(a => a.IdCursoHabilitado == idCursoHabilitado && a.Estado == "asignado")
                .Select(a => new
                {
                    IdEstudiante = a.IdEstudiante,
                    Carnet = a.IdEstudianteNavigation.Carnet,
                    NombreCompleto = $"{a.IdEstudianteNavigation.IdPersonaNavigation.PrimerNombre} {a.IdEstudianteNavigation.IdPersonaNavigation.PrimerApellido}",
                    Correo = ""
                })
                .ToListAsync();

            return participantes;
        }

        public async Task<object> GetEntregasPorTareaAsync(int idTarea)
        {
            var entregas = await _context.EntregaTareas
                .Include(e => e.IdEstudianteNavigation.IdPersonaNavigation)
                .Where(e => e.IdTarea == idTarea)
                .Select(e => new
                {
                    IdEntrega = e.IdEntrega,
                    NombreEstudiante = e.IdEstudianteNavigation.IdPersonaNavigation.PrimerNombre + " " + e.IdEstudianteNavigation.IdPersonaNavigation.PrimerApellido,
                    Carne = e.IdEstudianteNavigation.Carnet,
                    UrlArchivoAdjunto = e.UrlArchivoAdjunto,
                    FechaEnvio = e.FechaEnvio,
                    Calificacion = e.Calificacion,
                    Comentarios = e.ComentariosCatedratico
                }).ToListAsync();
            return entregas;
        }

        public async Task<(bool IsSuccess, string Message)> GenerarParcialesOficialesAsync(int idCursoHabilitado)
        {
            var curso = await _context.CursoHabilitados.FindAsync(idCursoHabilitado);
            if (curso == null) return (false, "Curso no encontrado.");
            var tareas = new List<Tarea>
            {
                new Tarea { IdCursoHabilitado = idCursoHabilitado, Titulo = "Primer Parcial", Descripcion = "Evaluación del primer módulo.", PunteoMaximo = 10, FechaCreacion = DateOnly.FromDateTime(DateTime.Now), FechaVencimiento = DateTime.Now.AddDays(7), Visibilidad = true },
                new Tarea { IdCursoHabilitado = idCursoHabilitado, Titulo = "Segundo Parcial", Descripcion = "Evaluación del segundo módulo.", PunteoMaximo = 10, FechaCreacion = DateOnly.FromDateTime(DateTime.Now), FechaVencimiento = DateTime.Now.AddDays(14), Visibilidad = true },
                new Tarea { IdCursoHabilitado = idCursoHabilitado, Titulo = "Tercer Parcial", Descripcion = "Evaluación del tercer módulo.", PunteoMaximo = 10, FechaCreacion = DateOnly.FromDateTime(DateTime.Now), FechaVencimiento = DateTime.Now.AddDays(21), Visibilidad = true },
                new Tarea { IdCursoHabilitado = idCursoHabilitado, Titulo = "Examen Final", Descripcion = "Evaluación final del curso.", PunteoMaximo = 30, FechaCreacion = DateOnly.FromDateTime(DateTime.Now), FechaVencimiento = DateTime.Now.AddDays(30), Visibilidad = true }
            };
            _context.Tareas.AddRange(tareas);
            await _context.SaveChangesAsync();
            return (true, "Evaluaciones oficiales generadas correctamente.");
        }
        public async Task<(bool IsSuccess, string Message)> GuardarAsistenciaCursoAsync(int idCursoHabilitado, AsistenciaGrupalDto dto)
        {
            var curso = await _context.CursoHabilitados.FindAsync(idCursoHabilitado);
            if (curso == null) return (false, "Curso no encontrado.");

            // Buscar si ya existe la sesión en esa fecha
            var sesion = await _context.ClaseSesions
                .FirstOrDefaultAsync(s => s.IdCursoHabilitado == idCursoHabilitado && s.FechaSesion == DateOnly.FromDateTime(dto.FechaSesion));

            if (sesion == null)
            {
                sesion = new ClaseSesion
                {
                    IdCursoHabilitado = idCursoHabilitado,
                    FechaSesion = DateOnly.FromDateTime(dto.FechaSesion),
                    TemaImpartido = "Sesión de clase"
                };
                _context.ClaseSesions.Add(sesion);
                await _context.SaveChangesAsync();
            }

            // Eliminar asistencias previas de esta sesión si existieran
            var asistenciasAnteriores = await _context.AsistenciaEstudiantes.Where(a => a.IdSesion == sesion.IdSesion).ToListAsync();
            if (asistenciasAnteriores.Any())
            {
                _context.AsistenciaEstudiantes.RemoveRange(asistenciasAnteriores);
            }

            var nuevasAsistencias = dto.Estudiantes.Select(e => new AsistenciaEstudiante
            {
                IdSesion = sesion.IdSesion,
                IdEstudiante = e.IdEstudiante,
                EstadoAsistencia = e.IsPresente ? "Presente" : "Ausente",
                FechaRegistro = DateTime.Now
            }).ToList();

            _context.AsistenciaEstudiantes.AddRange(nuevasAsistencias);
            await _context.SaveChangesAsync();

            return (true, "Asistencia guardada correctamente.");
        }
    }
}
