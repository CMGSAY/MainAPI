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
            var e = new CursoHabilitado { IdCarreraSemestreCurso = d.IdCarreraSemestreCurso, IdCiclo = d.IdCiclo, IdJornada = d.IdJornada, IdSeccion = d.IdSeccion, IdAula = d.IdAula, IdCatedratico = d.IdCatedratico, Estado = d.Estado ?? "activo" };
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
    }
}
