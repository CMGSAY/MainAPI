using MainAPI.Models.DTOs;
using MainAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Estudiante,Administrador")]
    public class PortalEstudianteController : ControllerBase
    {
        private readonly IPortalEstudianteService _estudianteService;

        public PortalEstudianteController(IPortalEstudianteService estudianteService)
        {
            _estudianteService = estudianteService;
        }

        private async Task<int> GetEstudianteId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return 0;
            int userId = int.Parse(userIdString);
            return await _estudianteService.GetEstudianteIdAsync(userId);
        }

        [HttpGet("mis-cursos")]
        public async Task<IActionResult> GetMisCursos()
        {
            int idEstudiante = await GetEstudianteId();
            if (idEstudiante == 0) return Unauthorized("Perfil de estudiante no encontrado.");

            var cursos = await _estudianteService.GetMisCursosAsync(idEstudiante);
            return Ok(cursos);
        }

        [HttpGet("curso/{idCursoHabilitado}/semanas")]
        public async Task<IActionResult> GetSemanasCurso(int idCursoHabilitado)
        {
            var result = await _estudianteService.GetSemanasCursoAsync(idCursoHabilitado);
            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok(result.Semanas);
        }

        [HttpPost("entregas")]
        public async Task<IActionResult> PostEntrega([FromBody] EntregaTareaDto d)
        {
            int idEstudiante = await GetEstudianteId();
            if (idEstudiante == 0) return Unauthorized("Perfil de estudiante no encontrado.");

            var result = await _estudianteService.PostEntregaAsync(idEstudiante, d);
            return Ok(new { mensaje = result.Message, result.IdEntrega });
        }

        [HttpGet("kardex")]
        public async Task<IActionResult> GetKardex()
        {
            int idEstudiante = await GetEstudianteId();
            if (idEstudiante == 0) return Unauthorized("Perfil de estudiante no encontrado.");

            var historial = await _estudianteService.GetKardexAsync(idEstudiante);
            return Ok(historial);
        }

        [HttpGet("cursos-disponibles-matricula")]
        public async Task<IActionResult> GetCursosDisponiblesMatricula()
        {
            int idEstudiante = await GetEstudianteId();
            if (idEstudiante == 0) return Unauthorized("Perfil de estudiante no encontrado.");

            var result = await _estudianteService.GetCursosDisponiblesMatriculaAsync(idEstudiante);
            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok(result.Cursos);
        }

        [HttpGet("{idEstudiante}/cursos-disponibles-matricula")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetCursosDisponiblesMatriculaAdmin(int idEstudiante)
        {
            var result = await _estudianteService.GetCursosDisponiblesMatriculaAsync(idEstudiante);
            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok(result.Cursos);
        }

        [HttpPost("matricularse")]
        public async Task<IActionResult> Matricularse([FromBody] int idCursoHabilitado)
        {
            int idEstudiante = await GetEstudianteId();
            if (idEstudiante == 0) return Unauthorized("Perfil de estudiante no encontrado.");

            var result = await _estudianteService.MatricularseAsync(idEstudiante, idCursoHabilitado);
            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok(new { mensaje = result.Message });
        }

        [HttpGet("curso/{idCursoHabilitado}/asistencias")]
        public async Task<IActionResult> GetAsistencias(int idCursoHabilitado)
        {
            int idEstudiante = await GetEstudianteId();
            if (idEstudiante == 0) return Unauthorized("Perfil de estudiante no encontrado.");

            var asistencias = await _estudianteService.GetAsistenciasAsync(idEstudiante, idCursoHabilitado);
            return Ok(asistencias);
        }

        [HttpGet("curso/{idCursoHabilitado}/calificaciones")]
        public async Task<IActionResult> GetCalificaciones(int idCursoHabilitado)
        {
            int idEstudiante = await GetEstudianteId();
            if (idEstudiante == 0) return Unauthorized("Perfil de estudiante no encontrado.");

            var calificaciones = await _estudianteService.GetCalificacionesAsync(idEstudiante, idCursoHabilitado);
            return Ok(calificaciones);
        }
        [HttpGet("tarea/{idTarea}/mi-entrega")]
        public async Task<IActionResult> GetMiEntrega(int idTarea)
        {
            int idEstudiante = await GetEstudianteId();
            if (idEstudiante == 0) return Unauthorized("Perfil de estudiante no encontrado.");

            var entrega = await _estudianteService.GetMiEntregaAsync(idEstudiante, idTarea);
            return Ok(entrega); // Puede regresar un 204 o nulo si no hay, la app lo leerá
        }
    }
}