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
    public class PerfilesController : ControllerBase
    {
        private readonly IPerfilesService _perfilesService;

        public PerfilesController(IPerfilesService perfilesService)
        {
            _perfilesService = perfilesService;
        }

        [HttpGet("estudiantes")]
        public async Task<IActionResult> GetEstudiantes()
        {
            return Ok(await _perfilesService.GetEstudiantesAsync());
        }

        [HttpGet("estudiantes/busqueda")]
        public async Task<IActionResult> GetAllEstudiantesBusqueda()
        {
            return Ok(await _perfilesService.GetAllEstudiantesBusquedaAsync());
        }

        [HttpPost("estudiantes")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PostEstudiante(PerfilEstudianteDto d)
        {
            return Ok(await _perfilesService.PostEstudianteAsync(d));
        }

        [HttpGet("catedraticos")]
        public async Task<IActionResult> GetCatedraticos()
        {
            return Ok(await _perfilesService.GetCatedraticosAsync());
        }

        [HttpPost("catedraticos")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PostCatedratico(PerfilCatedraticoDto d)
        {
            return Ok(await _perfilesService.PostCatedraticoAsync(d));
        }

        [HttpGet("admins")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAdmins()
        {
            return Ok(await _perfilesService.GetAdminsAsync());
        }

        [HttpPost("admins")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PostAdmin(PerfilAdministradorDto d)
        {
            return Ok(await _perfilesService.PostAdminAsync(d));
        }

        [HttpGet("catedraticos/busqueda")]
        public async Task<IActionResult> GetAllCatedraticosBusqueda()
        {
            return Ok(await _perfilesService.GetAllCatedraticosBusquedaAsync());
        }

        [HttpGet("validar-dpi/{dpi}")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidarDpiDuplicado(string dpi)
        {
            return Ok(await _perfilesService.ValidarDpiDuplicadoAsync(dpi));
        }

        [HttpGet("estudiantes/municipio/{idMunicipio}")]
        public async Task<IActionResult> GetEstudiantesByMunicipio(int idMunicipio)
        {
            return Ok(await _perfilesService.GetEstudiantesByMunicipioAsync(idMunicipio));
        }

        [HttpPut("estudiantes/{idEstudiante}/semestre/{idSemestre}")]
        public async Task<IActionResult> AsignarSemestre(int idEstudiante, int idSemestre)
        {
            var result = await _perfilesService.AsignarSemestreAsync(idEstudiante, idSemestre);
            if (!result.IsSuccess) return NotFound(result.Message);

            return Ok(new { message = result.Message });
        }

        public class RutaAcademicaDto
        {
            public int IdCarrera { get; set; }
            public int IdSemestre { get; set; }
        }

        [HttpPut("estudiantes/{idEstudiante}/ruta-academica")]
        public async Task<IActionResult> AsignarRutaAcademica(int idEstudiante, [FromBody] RutaAcademicaDto dto)
        {
            var result = await _perfilesService.AsignarRutaAcademicaAsync(idEstudiante, dto);
            if (!result.IsSuccess) return NotFound(result.Message);

            return Ok(new { message = result.Message });
        }
    }
}