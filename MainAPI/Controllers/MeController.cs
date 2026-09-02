using MainAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MeController : ControllerBase
    {
        private readonly MainDbContext _context;

        public MeController(MainDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMe()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int loginUserId))
            {
                return Unauthorized();
            }

            var persona = await _context.Personas
                .Where(p => p.LoginUserId == loginUserId)
                .Select(p => new
                {
                    Nombre = p.PrimerNombre,
                    NombreCompleto = p.PrimerNombre + " " + p.PrimerApellido
                })
                .FirstOrDefaultAsync();

            if (persona == null)
            {
                return NotFound();
            }

            return Ok(persona);
        }
    }
}
