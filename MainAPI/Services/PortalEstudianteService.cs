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
    public class PortalEstudianteService : IPortalEstudianteService
    {
        private readonly MainDbContext _context;

        public PortalEstudianteService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetEstudianteIdAsync(int loginUserId)
        {
            var estudiante = await _context.PerfilEstudiantes.FirstOrDefaultAsync(p => p.IdPersonaNavigation.LoginUserId == loginUserId);
            return estudiante?.IdEstudiante ?? 0;
        }

        public async Task<object?> GetMisCursosAsync(int idEstudiante)
        {
            var cursos = await (from a in _context.AsignacionCursos
                                join ch in _context.CursoHabilitados on a.IdCursoHabilitado equals ch.IdCursoHabilitado
                                join sec in _context.Seccions on ch.IdSeccion equals sec.IdSeccion
                                join csc in _context.CarreraSemestreCursos on ch.IdCarreraSemestreCurso equals csc.IdCarreraSemestreCurso
                                join c in _context.Cursos on csc.IdCurso equals c.IdCurso
                                join pc in _context.PerfilCatedraticos on ch.IdCatedratico equals pc.IdCatedratico into pcGroup
                                from pc in pcGroup.DefaultIfEmpty()
                                join per in _context.Personas on pc.IdPersona equals per.IdPersona into perGroup
                                from per in perGroup.DefaultIfEmpty()
                                where a.IdEstudiante == idEstudiante && ch.Estado == "activo"
                                select new
                                {
                                    IdCursoHabilitado = ch.IdCursoHabilitado,
                                    NombreCurso = c.NombreCurso,
                                    Seccion = sec.NombreSeccion,
                                    HorarioInicio = ch.HorarioInicio,
                                    HorarioFin = ch.HorarioFin,
                                    PrimerNombre = per != null ? per.PrimerNombre : "Sin",
                                    PrimerApellido = per != null ? per.PrimerApellido : "Asignar"
                                }).ToListAsync();

            var idsCursos = cursos.Select(c => c.IdCursoHabilitado).ToList();
            var horariosDb = new List<HorarioCurso>();
            
            if (idsCursos.Any())
            {
                horariosDb = await _context.HorarioCursos
                    .Where(hc => idsCursos.Contains(hc.IdCursoHabilitado))
                    .ToListAsync();
            }

            return cursos.Select(c => 
            {
                var dias = horariosDb.Where(h => h.IdCursoHabilitado == c.IdCursoHabilitado).Select(h => h.DiaSemana).ToList();
                string diasStr = dias.Any() ? string.Join(", ", dias) : "Sin días";

                return new
                {
                    c.IdCursoHabilitado,
                    c.NombreCurso,
                    c.Seccion,
                    Horario = c.HorarioInicio.HasValue && c.HorarioFin.HasValue 
                        ? $"{diasStr} | {c.HorarioInicio.Value.ToString("HH:mm")} - {c.HorarioFin.Value.ToString("HH:mm")}" 
                        : "Sin horario",
                    Docente = $"{c.PrimerNombre} {c.PrimerApellido}".Trim()
                };
            }).ToList();
        }

        public async Task<(bool IsSuccess, string Message, object? Semanas)> GetSemanasCursoAsync(int idCursoHabilitado)
        {
            var ciclo = await (from ch in _context.CursoHabilitados
                               join c in _context.CicloEscolars on ch.IdCiclo equals c.IdCiclo
                               where ch.IdCursoHabilitado == idCursoHabilitado
                               select c).FirstOrDefaultAsync();

            if (ciclo == null || ciclo.FechaInicio == null || ciclo.FechaFinalizacion == null)
                return (false, "El ciclo escolar no tiene fechas definidas.", null);

            DateTime fechaInicio = ciclo.FechaInicio.Value.ToDateTime(TimeOnly.MinValue);
            DateTime fechaFin = ciclo.FechaFinalizacion.Value.ToDateTime(TimeOnly.MinValue);

            var tareas = await _context.Tareas.Where(t => t.IdCursoHabilitado == idCursoHabilitado).ToListAsync();
            var materiales = await _context.MaterialClases.Where(m => m.IdCursoHabilitado == idCursoHabilitado).ToListAsync();

            var semanas = new List<object>();
            int semanaNumero = 1;
            DateTime inicioSemana = fechaInicio;

            while (inicioSemana <= fechaFin)
            {
                DateTime finSemana = inicioSemana.AddDays(6);
                if (finSemana > fechaFin) finSemana = fechaFin;

                DateOnly dateInicio = DateOnly.FromDateTime(inicioSemana);
                DateOnly dateFin = DateOnly.FromDateTime(finSemana);

                var tareasSemana = tareas.Where(t => t.FechaCreacion.HasValue && t.FechaCreacion.Value >= dateInicio && t.FechaCreacion.Value <= dateFin).ToList();
                var materialesSemana = materiales.Where(m => m.FechaSubida.HasValue && m.FechaSubida.Value >= dateInicio && m.FechaSubida.Value <= dateFin).ToList();

                semanas.Add(new
                {
                    NumeroSemana = semanaNumero,
                    Titulo = $"Semana {semanaNumero}",
                    Fechas = $"{inicioSemana:dd MMM} - {finSemana:dd MMM yyyy}",
                    FechaInicioSemana = inicioSemana,
                    FechaFinSemana = finSemana,
                    Tareas = tareasSemana,
                    Materiales = materialesSemana
                });

                inicioSemana = inicioSemana.AddDays(7);
                semanaNumero++;
            }

            return (true, "OK", semanas);
        }

        public async Task<(bool IsSuccess, string Message, int? IdEntrega)> PostEntregaAsync(int idEstudiante, EntregaTareaDto dto)
        {
            var e = new EntregaTarea
            {
                IdTarea = dto.IdTarea,
                IdEstudiante = idEstudiante,
                UrlArchivoAdjunto = dto.UrlArchivoAdjunto,
                FechaEnvio = DateTime.Now
            };
            _context.EntregaTareas.Add(e);
            await _context.SaveChangesAsync();
            return (true, "Tarea entregada correctamente", e.IdEntrega);
        }

        public async Task<object?> GetKardexAsync(int idEstudiante)
        {
            var historial = await _context.AsignacionCursos
                .Include(a => a.IdCursoHabilitadoNavigation.IdCarreraSemestreCursoNavigation.IdCursoNavigation)
                .Where(a => a.IdEstudiante == idEstudiante)
                .Select(a => new {
                    Curso = a.IdCursoHabilitadoNavigation.IdCarreraSemestreCursoNavigation.IdCursoNavigation.NombreCurso,
                    NotaFinal = a.NotaFinal,
                    Estado = a.NotaFinal >= 61 ? "Aprobado" : "Reprobado"
                }).ToListAsync();

            return historial;
        }

        public async Task<(bool IsSuccess, string Message, object? Cursos)> GetCursosDisponiblesMatriculaAsync(int idEstudiante)
        {
            var estudiante = await _context.PerfilEstudiantes.FindAsync(idEstudiante);
            if (estudiante == null || estudiante.IdSemestreActual == null || estudiante.IdCarrera == null)
                return (false, "No tienes un semestre o carrera oficializada por el Administrador.", null);

            var cursosDisponibles = await (from ch in _context.CursoHabilitados
                                           join csc in _context.CarreraSemestreCursos on ch.IdCarreraSemestreCurso equals csc.IdCarreraSemestreCurso
                                           join cs in _context.CarreraSemestres on csc.IdCarreraSemestre equals cs.IdCarreraSemestre
                                           join cur in _context.Cursos on csc.IdCurso equals cur.IdCurso
                                           join sec in _context.Seccions on ch.IdSeccion equals sec.IdSeccion
                                           join pc in _context.PerfilCatedraticos on ch.IdCatedratico equals pc.IdCatedratico
                                           join per in _context.Personas on pc.IdPersona equals per.IdPersona
                                           where cs.IdSemestre == estudiante.IdSemestreActual && cs.IdCarrera == estudiante.IdCarrera && ch.Estado == "activo"
                                           select new
                                           {
                                               IdCursoHabilitado = ch.IdCursoHabilitado,
                                               NombreCurso = cur.NombreCurso,
                                               Seccion = sec.NombreSeccion,
                                               PrimerNombre = per.PrimerNombre,
                                               PrimerApellido = per.PrimerApellido,
                                               HorarioInicio = ch.HorarioInicio,
                                               HorarioFin = ch.HorarioFin
                                           }).ToListAsync();

            var cursosResult = cursosDisponibles.Select(c => new
            {
                c.IdCursoHabilitado,
                c.NombreCurso,
                c.Seccion,
                Catedratico = $"{c.PrimerNombre} {c.PrimerApellido}".Trim(),
                Horario = $"{c.HorarioInicio?.ToString(@"hh\:mm") ?? "N/A"} - {c.HorarioFin?.ToString(@"hh\:mm") ?? "N/A"}"
            }).ToList();

            return (true, "OK", cursosResult);
        }

        public async Task<(bool IsSuccess, string Message)> MatricularseAsync(int idEstudiante, int idCursoHabilitado)
        {
            bool yaAsignado = await _context.AsignacionCursos.AnyAsync(a => a.IdCursoHabilitado == idCursoHabilitado && a.IdEstudiante == idEstudiante);
            if (yaAsignado) return (false, "Ya estás matriculado en este curso.");

            var asignacion = new AsignacionCurso
            {
                IdCursoHabilitado = idCursoHabilitado,
                IdEstudiante = idEstudiante,
                NotaFinal = 0
            };

            _context.AsignacionCursos.Add(asignacion);
            await _context.SaveChangesAsync();

            return (true, "Te has matriculado exitosamente.");
        }
    }
}
