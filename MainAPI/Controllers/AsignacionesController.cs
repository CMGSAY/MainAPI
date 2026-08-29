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
    [Authorize]
    public class AsignacionesController : ControllerBase
    {
        private readonly MainDbContext _context;
        public AsignacionesController(MainDbContext context) => _context = context;

        [HttpGet("cursos-habilitados")]
        public async Task<IActionResult> GetCursosHab() => Ok(await _context.CursoHabilitados.ToListAsync());

        [HttpGet("horarios-ocupados/{idSeccion}/{idAula}/{dia}")]
        public async Task<IActionResult> GetHorariosOcupados(int idSeccion, int idAula, string dia)
        {
            // Devuelve los horarios ocupados ya sea porque el Aula está ocupada o la Sección ya tiene clases.
            var horarios = await _context.HorarioCursos
                .Include(h => h.IdCursoHabilitadoNavigation)
                .Where(h => h.DiaSemana == dia &&
                           (h.IdCursoHabilitadoNavigation.IdSeccion == idSeccion || h.IdCursoHabilitadoNavigation.IdAula == idAula) &&
                           h.IdCursoHabilitadoNavigation.Estado == "activo")
                .Select(h => new { h.HoraInicio, h.HoraFin, h.IdCursoHabilitadoNavigation.IdSeccion, h.IdCursoHabilitadoNavigation.IdAula })
                .ToListAsync();
            return Ok(horarios);
        }

        [HttpPost("cursos-habilitados")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PostCursoHab(CursoHabilitadoDto d)
        {
            // 1. Validar Choques de Horario para la misma Sección y Ciclo
            if (d.Horarios != null && d.Horarios.Any())
            {
                foreach (var h in d.Horarios)
                {
                    var existeChoque = await _context.HorarioCursos
                        .Include(hc => hc.IdCursoHabilitadoNavigation)
                        .AnyAsync(hc => hc.IdCursoHabilitadoNavigation.IdSeccion == d.IdSeccion &&
                                        hc.IdCursoHabilitadoNavigation.IdCiclo == d.IdCiclo &&
                                        hc.DiaSemana == h.DiaSemana &&
                                        ((h.HoraInicio >= hc.HoraInicio && h.HoraInicio < hc.HoraFin) ||
                                         (h.HoraFin > hc.HoraInicio && h.HoraFin <= hc.HoraFin) ||
                                         (h.HoraInicio <= hc.HoraInicio && h.HoraFin >= hc.HoraFin)));

                    if (existeChoque)
                    {
                        return BadRequest($"Choque de horario detectado: La sección ya tiene clases el {h.DiaSemana} en ese horario.");
                    }
                }
            }

            // 2. Guardar el Curso Habilitado
            var e = new CursoHabilitado
            {
                IdCarreraSemestreCurso = d.IdCarreraSemestreCurso,
                IdCiclo = d.IdCiclo,
                IdJornada = d.IdJornada,
                IdSeccion = d.IdSeccion,
                IdAula = d.IdAula,
                IdCatedratico = d.IdCatedratico,
                Estado = d.Estado ?? "activo",
                HorarioInicio = d.Horarios?.FirstOrDefault()?.HoraInicio,
                HorarioFin = d.Horarios?.FirstOrDefault()?.HoraFin
            };
            _context.CursoHabilitados.Add(e);
            await _context.SaveChangesAsync();

            // 3. Guardar los Horarios
            if (d.Horarios != null && d.Horarios.Any())
            {
                foreach (var h in d.Horarios)
                {
                    _context.HorarioCursos.Add(new HorarioCurso
                    {
                        IdCursoHabilitado = e.IdCursoHabilitado,
                        DiaSemana = h.DiaSemana,
                        HoraInicio = h.HoraInicio,
                        HoraFin = h.HoraFin
                    });
                }
                await _context.SaveChangesAsync();
            }

            return Ok(e);
        }

        [HttpGet("estudiantes")]
        [Authorize(Roles = "Administrador,Docente")]
        public async Task<IActionResult> GetAsignaciones() => Ok(await _context.AsignacionCursos.ToListAsync());

        [HttpPost("estudiantes")]
        [Authorize(Roles = "Administrador,Estudiante")]
        public async Task<IActionResult> PostAsignacion(AsignacionCursoDto d)
        {
            var e = new AsignacionCurso { IdEstudiante = d.IdEstudiante, IdCursoHabilitado = d.IdCursoHabilitado, FechaAsignacion = DateOnly.FromDateTime(DateTime.Now), Estado = "asignado", NotaFinal = 0 };
            _context.AsignacionCursos.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpPost("matricula-multiple")]
        [Authorize(Roles = "Administrador,Estudiante")]
        public async Task<IActionResult> PostMatriculaMultiple(MatriculaMultipleDto d)
        {
            var conf = await _context.ConfiguracionSistemas.FirstOrDefaultAsync(c => c.Clave == "inscripciones_abiertas");
            if (conf == null || conf.Valor != "true")
            {
                return BadRequest("El proceso de inscripción/asignación está cerrado actualmente.");
            }

            foreach (var idCurso in d.IdsCursosHabilitados)
            {
                // Validación de Cupos (Límite 40)
                int inscritos = await _context.AsignacionCursos.CountAsync(a => a.IdCursoHabilitado == idCurso);
                if (inscritos >= 40)
                {
                    return BadRequest($"Cupo máximo alcanzado (40) para el curso ID {idCurso}. Por favor asigne otra sección.");
                }

                var e = new AsignacionCurso { IdEstudiante = d.IdEstudiante, IdCursoHabilitado = idCurso, FechaAsignacion = DateOnly.FromDateTime(DateTime.Now), Estado = "asignado", NotaFinal = 0 };
                _context.AsignacionCursos.Add(e);
            }
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Cursos matriculados exitosamente" });
        }

        [HttpGet("cursos-habilitados/curso-pensum/{idCarreraSemestreCurso}")]
        public async Task<IActionResult> GetCursosHabilitadosPorPensum(int idCarreraSemestreCurso)
        {
            // Usamos JOINs seguros en lugar de Includes para evitar errores de propiedades de navegación faltantes
            var res = await (from ch in _context.CursoHabilitados
                             join sec in _context.Seccions on ch.IdSeccion equals sec.IdSeccion
                             join pc in _context.PerfilCatedraticos on ch.IdCatedratico equals pc.IdCatedratico
                             join per in _context.Personas on pc.IdPersona equals per.IdPersona
                             where ch.IdCarreraSemestreCurso == idCarreraSemestreCurso && ch.Estado == "activo"
                             select new
                             {
                                 IdCursoHabilitado = ch.IdCursoHabilitado,
                                 DescripcionLarga = $"Sección {sec.NombreSeccion} - Docente: {per.PrimerNombre} {per.PrimerApellido} ({ch.HorarioInicio}-{ch.HorarioFin})"
                             }).ToListAsync();

            return Ok(res);
        }
    }
}