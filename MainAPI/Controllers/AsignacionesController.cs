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
            try
            {
                // Devuelve los horarios ocupados ya sea porque el Aula está ocupada o la Sección ya tiene clases.
                var horariosDb = await _context.HorarioCursos
                    .Include(h => h.IdCursoHabilitadoNavigation)
                    .Where(h => h.DiaSemana == dia &&
                               (h.IdCursoHabilitadoNavigation.IdSeccion == idSeccion || h.IdCursoHabilitadoNavigation.IdAula == idAula) &&
                               h.IdCursoHabilitadoNavigation.Estado == "activo")
                    .Select(h => new { h.HoraInicio, h.HoraFin, h.IdCursoHabilitadoNavigation.IdSeccion, h.IdCursoHabilitadoNavigation.IdAula })
                    .ToListAsync();

                var horarios = horariosDb.Select(h => new {
                    HoraInicio = h.HoraInicio.ToString("HH:mm:ss"),
                    HoraFin = h.HoraFin.ToString("HH:mm:ss"),
                    h.IdSeccion,
                    h.IdAula
                }).ToList();

                return Ok(horarios);
            }
            catch (Exception ex)
            {
                Console.WriteLine("===== ERROR EN GET HORARIOS OCUPADOS =====");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }


        [HttpPost("cursos-habilitados")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PostCursoHab(CursoHabilitadoDto d)
        {
            try
            {
                // 1. Validar Choques de Horario para la misma Sección, Aula y Ciclo
                if (d.Horarios != null && d.Horarios.Any())
                {
                    foreach (var h in d.Horarios)
                    {
                        var reqInicio = TimeOnly.Parse(h.HoraInicio);
                        var reqFin = TimeOnly.Parse(h.HoraFin);

                        var choqueSeccion = await _context.HorarioCursos
                            .Include(hc => hc.IdCursoHabilitadoNavigation)
                            .AnyAsync(hc => hc.IdCursoHabilitadoNavigation.IdSeccion == d.IdSeccion &&
                                            hc.IdCursoHabilitadoNavigation.IdCiclo == d.IdCiclo &&
                                            hc.IdCursoHabilitadoNavigation.Estado == "activo" &&
                                            hc.DiaSemana == h.DiaSemana &&
                                            ((reqInicio >= hc.HoraInicio && reqInicio < hc.HoraFin) ||
                                             (reqFin > hc.HoraInicio && reqFin <= hc.HoraFin) ||
                                             (reqInicio <= hc.HoraInicio && reqFin >= hc.HoraFin)));

                        if (choqueSeccion) return BadRequest($"Choque de horario detectado: La sección ya tiene clases el {h.DiaSemana} en ese horario.");

                        var choqueAula = await _context.HorarioCursos
                            .Include(hc => hc.IdCursoHabilitadoNavigation)
                            .AnyAsync(hc => hc.IdCursoHabilitadoNavigation.IdAula == d.IdAula &&
                                            hc.IdCursoHabilitadoNavigation.IdCiclo == d.IdCiclo &&
                                            hc.IdCursoHabilitadoNavigation.Estado == "activo" &&
                                            hc.DiaSemana == h.DiaSemana &&
                                            ((reqInicio >= hc.HoraInicio && reqInicio < hc.HoraFin) ||
                                             (reqFin > hc.HoraInicio && reqFin <= hc.HoraFin) ||
                                             (reqInicio <= hc.HoraInicio && reqFin >= hc.HoraFin)));

                        if (choqueAula) return BadRequest($"Choque de horario detectado: El Aula/Salón ya está ocupada el {h.DiaSemana} en ese horario.");
                    }
                }

                // 2. Guardar el Curso Habilitado
                var primerHorario = d.Horarios?.FirstOrDefault();
                var e = new CursoHabilitado
                {
                    IdCarreraSemestreCurso = d.IdCarreraSemestreCurso,
                    IdCiclo = d.IdCiclo,
                    IdJornada = d.IdJornada,
                    IdSeccion = d.IdSeccion,
                    IdAula = d.IdAula,
                    IdCatedratico = d.IdCatedratico,
                    Estado = d.Estado ?? "activo",
                    HorarioInicio = primerHorario != null ? TimeOnly.Parse(primerHorario.HoraInicio) : null,
                    HorarioFin = primerHorario != null ? TimeOnly.Parse(primerHorario.HoraFin) : null
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
                            HoraInicio = TimeOnly.Parse(h.HoraInicio),
                            HoraFin = TimeOnly.Parse(h.HoraFin)
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                return Ok(new { mensaje = "Curso habilitado exitosamente" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("===== ERROR EN POST CURSO HABILITADO =====");
                Console.WriteLine(ex.ToString());

                // Capturar el error real de la base de datos para saber qué está fallando (Foreign Key, Null reference, etc)
                var errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, $"Error interno: {errorMsg}");
            }
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

        [HttpGet("cursos-habilitados/activos")]
        public async Task<IActionResult> GetCursosActivos()
        {
            var res = await (from ch in _context.CursoHabilitados
                             join sec in _context.Seccions on ch.IdSeccion equals sec.IdSeccion
                             join csc in _context.CarreraSemestreCursos on ch.IdCarreraSemestreCurso equals csc.IdCarreraSemestreCurso
                             join c in _context.Cursos on csc.IdCurso equals c.IdCurso
                             join pc in _context.PerfilCatedraticos on ch.IdCatedratico equals pc.IdCatedratico
                             join per in _context.Personas on pc.IdPersona equals per.IdPersona
                             where ch.Estado == "activo"
                             select new
                             {
                                 IdCursoHabilitado = ch.IdCursoHabilitado,
                                 DisplayString = $"{c.NombreCurso} (Sec: {sec.NombreSeccion}) - {per.PrimerNombre} {per.PrimerApellido}"
                             }).ToListAsync();
            return Ok(res);
        }

        [HttpPut("cursos-habilitados/{id}/desactivar")]
        public async Task<IActionResult> DesactivarCurso(int id)
        {
            var curso = await _context.CursoHabilitados.FindAsync(id);
            if (curso == null) return NotFound("Curso no encontrado.");

            curso.Estado = "inactivo";
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Curso deshabilitado exitosamente. Los horarios han sido liberados." });
        }
    }
}