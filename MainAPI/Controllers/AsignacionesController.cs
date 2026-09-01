using MainAPI.Models.DTOs;
using MainAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AsignacionesController : ControllerBase
    {
        private readonly IAsignacionesService _asignacionesService;

        public AsignacionesController(IAsignacionesService asignacionesService)
        {
            _asignacionesService = asignacionesService;
        }

        [HttpGet("cursos-habilitados")]
        public async Task<IActionResult> GetCursosHab()
        {
            return Ok(await _asignacionesService.GetCursosHabilitadosAsync());
        }

        [HttpGet("horarios-ocupados/{idSeccion}/{idAula}/{dia}")]
        public async Task<IActionResult> GetHorariosOcupados(int idSeccion, int idAula, string dia)
        {
            var horarios = await _asignacionesService.GetHorariosOcupadosAsync(idSeccion, idAula, dia);
            return Ok(horarios);
        }

        [HttpPost("cursos-habilitados")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PostCursoHab(CursoHabilitadoDto d)
        {
            var result = await _asignacionesService.PostCursoHabilitadoAsync(d);
            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok(new { mensaje = result.Message });
        }

        [HttpGet("estudiantes")]
        [Authorize(Roles = "Administrador,Docente")]
        public async Task<IActionResult> GetAsignaciones()
        {
            return Ok(await _asignacionesService.GetAsignacionesAsync());
        }

        [HttpPost("estudiantes")]
        [Authorize(Roles = "Administrador,Estudiante")]
        public async Task<IActionResult> PostAsignacion(AsignacionCursoDto d)
        {
            var asignacion = await _asignacionesService.PostAsignacionAsync(d);
            return Ok(asignacion);
        }

        [HttpPost("matricula-multiple")]
        [Authorize(Roles = "Administrador,Estudiante")]
        public async Task<IActionResult> PostMatriculaMultiple(MatriculaMultipleDto d)
        {
            var result = await _asignacionesService.PostMatriculaMultipleAsync(d);
            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok(new { mensaje = result.Message });
        }

        [HttpGet("cursos-habilitados/curso-pensum/{idCarreraSemestreCurso}")]
        public async Task<IActionResult> GetCursosHabilitadosPorPensum(int idCarreraSemestreCurso)
        {
            var res = await _asignacionesService.GetCursosHabilitadosPorPensumAsync(idCarreraSemestreCurso);
            return Ok(res);
        }

        [HttpGet("cursos-habilitados/activos")]
        public async Task<IActionResult> GetCursosActivos()
        {
            var res = await _asignacionesService.GetCursosActivosAsync();
            return Ok(res);
        }

        [HttpPut("cursos-habilitados/{id}/desactivar")]
        public async Task<IActionResult> DesactivarCurso(int id)
        {
            var result = await _asignacionesService.DesactivarCursoAsync(id);
            if (!result.IsSuccess) return NotFound(result.Message);

            return Ok(new { mensaje = result.Message });
        }
    }
}