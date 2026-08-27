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

        [HttpPost("cursos-habilitados")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PostCursoHab(CursoHabilitadoDto d)
        {
            var e = new CursoHabilitado
            {
                IdCarreraSemestreCurso = d.IdCarreraSemestreCurso,
                IdCiclo = d.IdCiclo,
                IdJornada = d.IdJornada,
                IdSeccion = d.IdSeccion,
                IdAula = d.IdAula,
                IdCatedratico = d.IdCatedratico,
                Estado = d.Estado ?? "activo"
            };
            _context.CursoHabilitados.Add(e);
            await _context.SaveChangesAsync();
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
                                 // AQUÍ ESTÁ LA MAGIA: Ya no le pedimos el Horario
                                 DescripcionLarga = $"Sección {sec.NombreSeccion} - Docente: {per.PrimerNombre} {per.PrimerApellido}"
                             }).ToListAsync();

            return Ok(res);
        }
    }
}
