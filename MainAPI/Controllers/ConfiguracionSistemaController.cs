using MainAPI.Data;
using MainAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class ConfiguracionSistemaController : ControllerBase
    {
        private readonly MainDbContext _context;
        public ConfiguracionSistemaController(MainDbContext context) => _context = context;

        public class ConfigDto { public string Clave { get; set; } public string Valor { get; set; } }

        [HttpGet]
        public async Task<IActionResult> GetConfiguraciones()
        {
            return Ok(await _context.ConfiguracionSistemas.ToListAsync());
        }

        [HttpPost("guardar")]
        public async Task<IActionResult> GuardarConfiguracion([FromBody] ConfigDto dto)
        {
            var config = await _context.ConfiguracionSistemas.FirstOrDefaultAsync(c => c.Clave == dto.Clave);
            if (config != null)
            {
                config.Valor = dto.Valor;
            }
            else
            {
                _context.ConfiguracionSistemas.Add(new ConfiguracionSistema { Clave = dto.Clave, Valor = dto.Valor });
            }

            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Configuración guardada correctamente." });
        }
    }
}