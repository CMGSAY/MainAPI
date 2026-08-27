using MainAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConfiguracionController : ControllerBase
    {
        private readonly MainDbContext _context;
        public ConfiguracionController(MainDbContext context) => _context = context;

        [HttpGet("inscripciones-abiertas")]
        public async Task<IActionResult> GetInscripcionesAbiertas()
        {
            var conf = await _context.ConfiguracionSistemas.FirstOrDefaultAsync(c => c.Clave == "inscripciones_abiertas");
            return Ok(new { Abierto = conf != null && conf.Valor == "true" });
        }

        [HttpPut("inscripciones-abiertas")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ToggleInscripcionesAbiertas([FromBody] bool abierto)
        {
            var conf = await _context.ConfiguracionSistemas.FirstOrDefaultAsync(c => c.Clave == "inscripciones_abiertas");
            if (conf == null)
            {
                conf = new Models.ConfiguracionSistema { Clave = "inscripciones_abiertas", Valor = abierto ? "true" : "false" };
                _context.ConfiguracionSistemas.Add(conf);
            }
            else
            {
                conf.Valor = abierto ? "true" : "false";
            }
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = abierto ? "Inscripciones Abiertas" : "Inscripciones Cerradas" });
        }
    }
}