using MainAPI.Data;
using MainAPI.Models;
using MainAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Docente,Administrador")]
    public class DocenteController : ControllerBase
    {
        private readonly MainDbContext _context;
        public DocenteController(MainDbContext context) => _context = context;

        private async Task<int> GetDocenteId()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return 0;
            var userId = int.Parse(userIdString);

            var perfil = await _context.PerfilCatedraticos.FirstOrDefaultAsync(p => p.IdPersonaNavigation.LoginUserId == userId);
            return perfil?.IdCatedratico ?? 0;
        }

        // 1. OBTENER CURSOS ASIGNADOS AL DOCENTE
        [HttpGet("mis-cursos")]
        public async Task<IActionResult> GetMisCursos()
        {
            int idDocente = await GetDocenteId();
            if (idDocente == 0) return Unauthorized("Perfil de Catedrático no encontrado.");

            var cursos = await (from ch in _context.CursoHabilitados
                                join sec in _context.Seccions on ch.IdSeccion equals sec.IdSeccion
                                join csc in _context.CarreraSemestreCursos on ch.IdCarreraSemestreCurso equals csc.IdCarreraSemestreCurso
                                join cs in _context.CarreraSemestres on csc.IdCarreraSemestre equals cs.IdCarreraSemestre
                                join sem in _context.Semestres on cs.IdSemestre equals sem.IdSemestre
                                join c in _context.Cursos on csc.IdCurso equals c.IdCurso
                                join car in _context.Carreras on cs.IdCarrera equals car.IdCarrera
                                join fac in _context.Facultads on car.IdFacultad equals fac.IdFacultad
                                join ciclo in _context.CicloEscolars on ch.IdCiclo equals ciclo.IdCiclo
                                where ch.IdCatedratico == idDocente
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
                                    EsHistorico = (ch.Estado != "activo" || !ciclo.Estado.GetValueOrDefault())
                                }).ToListAsync();

            var result = cursos.Select(c => new
            {
                c.IdCursoHabilitado,
                c.NombreCurso,
                c.Seccion,
                c.Carrera,
                c.Facultad,
                c.Semestre,
                Horario = $"{c.HorarioInicio?.ToString(@"hh\:mm") ?? "N/A"} - {c.HorarioFin?.ToString(@"hh\:mm") ?? "N/A"}",
                c.Estado,
                c.EsHistorico
            }).ToList();

            return Ok(result);
        }

        // 2. OBTENER EL CURSO POR SEMANAS (Algoritmo de Agrupación)
        [HttpGet("curso/{idCursoHabilitado}/semanas")]
        public async Task<IActionResult> GetSemanasCurso(int idCursoHabilitado)
        {
            var cursoHab = await _context.CursoHabilitados
                .Include(c => c.IdCicloNavigation)
                .FirstOrDefaultAsync(c => c.IdCursoHabilitado == idCursoHabilitado);

            if (cursoHab == null) return NotFound();

            var fechaInicioCiclo = cursoHab.IdCicloNavigation.FechaInicio ?? DateOnly.FromDateTime(DateTime.Now);
            var fechaFinCiclo = cursoHab.IdCicloNavigation.FechaFinalizacion ?? DateOnly.FromDateTime(DateTime.Now.AddMonths(6));

            var materiales = await _context.MaterialClases.Where(m => m.IdCursoHabilitado == idCursoHabilitado).ToListAsync();
            var tareas = await _context.Tareas.Where(t => t.IdCursoHabilitado == idCursoHabilitado).ToListAsync();

            var semanas = new List<object>();
            var semanaActualIndex = 0;
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

                var tareasSemana = tareas.Where(t => t.FechaVencimiento >= currentDate &&
                    t.FechaVencimiento <= endOfWeek.AddDays(1).AddSeconds(-1)).ToList();

                semanas.Add(new
                {
                    NumeroSemana = i,
                    Titulo = $"{currentDate:dd MMM} - {endOfWeek:dd MMM}",
                    EsSemanaActual = isCurrent,
                    Materiales = materialesSemana.Select(m => new { m.IdMaterial, m.Titulo, m.UrlDocumento }),
                    Tareas = tareasSemana.Select(t => new { t.IdTarea, t.Titulo, t.FechaVencimiento, t.PunteoMaximo })
                });

                currentDate = currentDate.AddDays(7);
                i++;
            }

            return Ok(semanas);
        }

        // DTO PARA CREAR TAREA (Evita errores de validación de propiedades de navegación)
        public class CrearTareaDto
        {
            public string Titulo { get; set; } = null!;
            public string? Descripcion { get; set; }
            public decimal PunteoMaximo { get; set; }
            public DateTime FechaVencimiento { get; set; }
            public bool Visibilidad { get; set; }
        }

        // 3. CREAR TAREA (Con Validación de Tope de Puntos)
        [HttpPost("curso/{idCursoHabilitado}/tarea")]
        public async Task<IActionResult> PostTarea(int idCursoHabilitado, [FromBody] CrearTareaDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Validar que la sumatoria de todas las tareas y parciales no exceda los 100 puntos (o el tope del curso)
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
                return BadRequest($"Error: La creación de esta tarea ({dto.PunteoMaximo} pts) excede el máximo del curso ({tope} pts). Puntos ocupados actuales: {puntosActualesTareas + puntosParciales}.");
            }

            var nuevaTarea = new Tarea
            {
                IdCursoHabilitado = idCursoHabilitado,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                PunteoMaximo = dto.PunteoMaximo,
                FechaVencimiento = dto.FechaVencimiento,
                Visibilidad = dto.Visibilidad,
                FechaCreacion = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Tareas.Add(nuevaTarea);
            await _context.SaveChangesAsync();
            return Ok(nuevaTarea);
        }

        // 4. GRADEBOOK (LIBRO DE CALIFICACIONES EXPERTO)
        [HttpGet("curso/{idCursoHabilitado}/gradebook")]
        public async Task<IActionResult> GetGradebook(int idCursoHabilitado)
        {
            // Obtener variables globales de configuración
            var confZona = await _context.ConfiguracionSistemas.FirstOrDefaultAsync(c => c.Clave == "zona_minima_examen");
            var confAsist = await _context.ConfiguracionSistemas.FirstOrDefaultAsync(c => c.Clave == "asistencia_minima_porcentaje");
            var confAprob = await _context.ConfiguracionSistemas.FirstOrDefaultAsync(c => c.Clave == "nota_minima_aprobacion");

            decimal zonaMinima = confZona != null ? decimal.Parse(confZona.Valor) : 31m;
            decimal asistenciaMinima = confAsist != null ? decimal.Parse(confAsist.Valor) : 80m;
            decimal notaAprobacion = confAprob != null ? decimal.Parse(confAprob.Valor) : 61m;

            // Obtener todos los alumnos inscritos
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
                // Calcular Asistencia (solo de clases posteriores a su fecha de asignación)
                var clasesValidas = sesionesTotales.Where(s => s.FechaSesion >= a.FechaAsignacion).ToList();
                int totalClases = clasesValidas.Count;
                decimal porcentajeAsistencia = 100m; // Asumir 100% si no hay clases

                if (totalClases > 0)
                {
                    var idSesionesValidas = clasesValidas.Select(s => s.IdSesion).ToList();
                    var asistencias = await _context.AsistenciaEstudiantes
                        .Where(ast => ast.IdEstudiante == a.IdEstudiante && idSesionesValidas.Contains(ast.IdSesion) && (ast.EstadoAsistencia == "Presente" || ast.EstadoAsistencia == "Excusa" || ast.EstadoAsistencia == "Tarde"))
                        .CountAsync();

                    porcentajeAsistencia = ((decimal)asistencias / totalClases) * 100m;
                }

                // Calcular Zona
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

                // Si no tiene derecho a examen, la nota final ingresada se anula lógicamente (o se bloquea)
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

            return Ok(gradebook);
        }

        // 5. CALIFICAR UNA ENTREGA (Usando Porcentajes)
        public class CalificarDto { public decimal PorcentajeObtenido { get; set; } public string? Comentarios { get; set; } }

        [HttpPut("entrega/{idEntrega}/calificar")]
        public async Task<IActionResult> PutCalificarEntrega(int idEntrega, [FromBody] CalificarDto d)
        {
            var entrega = await _context.EntregaTareas.Include(e => e.IdTareaNavigation).FirstOrDefaultAsync(e => e.IdEntrega == idEntrega);
            if (entrega == null) return NotFound("Entrega no encontrada");

            // Cálculo matemático en el servidor
            entrega.PorcentajeObtenido = d.PorcentajeObtenido;
            entrega.Calificacion = (entrega.IdTareaNavigation.PunteoMaximo * d.PorcentajeObtenido) / 100m;
            entrega.ComentariosCatedratico = d.Comentarios;

            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Calificación guardada.", PuntosCalculados = entrega.Calificacion });
        }

        // 6. OBTENER PARTICIPANTES DEL CURSO
        [HttpGet("{idCursoHabilitado}/participantes")]
        public async Task<IActionResult> GetParticipantes(int idCursoHabilitado)
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

            return Ok(participantes);
        }
    }
}