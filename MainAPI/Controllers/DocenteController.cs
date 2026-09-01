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
    [Authorize(Roles = "Docente,Administrador")]
    public class DocenteController : ControllerBase
    {
        private readonly IDocenteService _docenteService;

        public DocenteController(IDocenteService docenteService)
        {
            _docenteService = docenteService;
        }

        private async Task<int> GetDocenteId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return 0;
            var userId = int.Parse(userIdString);
            return await _docenteService.GetDocenteIdAsync(userId);
        }

        [HttpGet("mis-cursos")]
        public async Task<IActionResult> GetMisCursos()
        {
            int idDocente = await GetDocenteId();
            if (idDocente == 0) return Unauthorized("Perfil de Catedrático no encontrado.");

            var cursos = await _docenteService.GetMisCursosAsync(idDocente);
            return Ok(cursos);
        }

        [HttpGet("curso/{idCursoHabilitado}/semanas")]
        public async Task<IActionResult> GetSemanasCurso(int idCursoHabilitado)
        {
            var semanas = await _docenteService.GetSemanasCursoAsync(idCursoHabilitado);
            if (semanas == null) return NotFound();
            return Ok(semanas);
        }

        [HttpPost("curso/{idCursoHabilitado}/tarea")]
        public async Task<IActionResult> PostTarea(int idCursoHabilitado, [FromBody] CrearTareaDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _docenteService.CrearTareaAsync(idCursoHabilitado, dto);
            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok(result.Tarea);
        }

        [HttpPost("curso/{idCursoHabilitado}/material")]
        public async Task<IActionResult> PostMaterial(int idCursoHabilitado, [FromBody] CrearMaterialDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var material = await _docenteService.CrearMaterialAsync(idCursoHabilitado, dto);
            return Ok(material);
        }

        [HttpGet("curso/{idCursoHabilitado}/gradebook")]
        public async Task<IActionResult> GetGradebook(int idCursoHabilitado)
        {
            var gradebook = await _docenteService.GetGradebookAsync(idCursoHabilitado);
            return Ok(gradebook);
        }

        [HttpPut("entrega/{idEntrega}/calificar-porcentaje")]
        public async Task<IActionResult> PutCalificarEntrega(int idEntrega, [FromBody] CalificarDto dto)
        {
            var result = await _docenteService.CalificarPorPorcentajeAsync(idEntrega, dto);
            if (!result.IsSuccess) return NotFound(result.Message);

            return Ok(new { mensaje = result.Message, PuntosCalculados = result.PuntosCalculados });
        }

        [HttpPut("entrega/{idEntrega}/calificar")]
        public async Task<IActionResult> PutCalificacion(int idEntrega, [FromBody] CalificacionDto dto)
        {
            var result = await _docenteService.CalificarDirectoAsync(idEntrega, dto);
            if (!result.IsSuccess) return NotFound(result.Message);

            return Ok(new { mensaje = result.Message });
        }

        [HttpGet("{idCursoHabilitado}/participantes")]
        public async Task<IActionResult> GetParticipantes(int idCursoHabilitado)
        {
            var participantes = await _docenteService.GetParticipantesAsync(idCursoHabilitado);
            return Ok(participantes);
        }

        [HttpGet("tarea/{idTarea}/entregas")]
        public async Task<IActionResult> GetEntregasPorTarea(int idTarea)
        {
            var entregas = await _docenteService.GetEntregasPorTareaAsync(idTarea);
            return Ok(entregas);
        }

        [HttpPost("curso/{idCursoHabilitado}/generar-parciales")]
        public async Task<IActionResult> PostGenerarParciales(int idCursoHabilitado)
        {
            var result = await _docenteService.GenerarParcialesOficialesAsync(idCursoHabilitado);
            if (!result.IsSuccess) return NotFound(result.Message);

            return Ok(new { mensaje = result.Message });
        }
    }
}