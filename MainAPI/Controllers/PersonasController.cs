using MainAPI.Models.DTOs;
using MainAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class PersonasController : ControllerBase
    {
        private readonly IPersonasService _personasService;

        public PersonasController(IPersonasService personasService)
        {
            _personasService = personasService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _personasService.GetPersonasAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post(PersonaDto d)
        {
            return Ok(await _personasService.PostPersonaAsync(d));
        }

        [HttpGet("gestion-usuarios")]
        public async Task<IActionResult> GetGestionUsuarios()
        {
            return Ok(await _personasService.GetGestionUsuariosAsync());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deshabilitar(int id)
        {
            var result = await _personasService.DeshabilitarPersonaAsync(id);
            if (!result.IsSuccess) return NotFound(new { Mensaje = result.Message });

            return Ok(new { Mensaje = result.Message });
        }

        [HttpPut("{id}/habilitar")]
        public async Task<IActionResult> Habilitar(int id)
        {
            var result = await _personasService.HabilitarPersonaAsync(id);
            if (!result.IsSuccess) return NotFound(new { Mensaje = result.Message });

            return Ok(new { Mensaje = result.Message });
        }
    }
}