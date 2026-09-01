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
    public class PortalDocenteController : ControllerBase
    {
        private readonly IPortalDocenteService _docenteService;

        public PortalDocenteController(IPortalDocenteService docenteService)
        {
            _docenteService = docenteService;
        }

        private async Task<int> GetCatedraticoId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return 0;
            int userId = int.Parse(userIdString);
            return await _docenteService.GetCatedraticoIdAsync(userId);
        }

        [HttpGet("mis-cursos")]
        public async Task<IActionResult> GetMisCursos()
        {
            int idCatedratico = await GetCatedraticoId();
            if (idCatedratico == 0) return Unauthorized("Perfil de catedrático no encontrado o Token inválido.");

            return Ok(await _docenteService.GetMisCursosAsync(idCatedratico));
        }

        [HttpGet("materiales")]
        public async Task<IActionResult> GetMateriales()
        {
            return Ok(await _docenteService.GetMaterialesAsync());
        }

        [HttpPost("materiales")]
        public async Task<IActionResult> PostMaterial(MaterialDto d)
        {
            return Ok(await _docenteService.PostMaterialAsync(d));
        }

        [HttpGet("tareas")]
        public async Task<IActionResult> GetTareas()
        {
            return Ok(await _docenteService.GetTareasAsync());
        }

        [HttpPost("tareas")]
        public async Task<IActionResult> PostTarea(TareaDto d)
        {
            return Ok(await _docenteService.PostTareaAsync(d));
        }

        [HttpGet("entregas")]
        public async Task<IActionResult> GetEntregas()
        {
            return Ok(await _docenteService.GetEntregasAsync());
        }

        [HttpPut("entregas/{id}/calificar")]
        public async Task<IActionResult> CalificarEntrega(int id, CalificacionTareaDto d)
        {
            var result = await _docenteService.CalificarEntregaAsync(id, d);
            if (!result.IsSuccess) return NotFound();

            return Ok(result.Result);
        }

        [HttpGet("evaluaciones")]
        public async Task<IActionResult> GetEvaluaciones()
        {
            return Ok(await _docenteService.GetEvaluacionesAsync());
        }

        [HttpPost("evaluaciones")]
        public async Task<IActionResult> PostEvaluacion(EvaluacionFijaDto d)
        {
            return Ok(await _docenteService.PostEvaluacionAsync(d));
        }

        [HttpPost("evaluaciones/notas")]
        public async Task<IActionResult> PostNota(CalificacionEvaluacionDto d)
        {
            return Ok(await _docenteService.PostNotaAsync(d));
        }
    }
}