using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // <--- ¡Esta es la magia! Protege todo el controlador
    public class TestAuthController : ControllerBase
    {
        [HttpGet("quien-soy")]
        public IActionResult QuienSoy()
        {
            // Extraer datos del JWT
            var correo = User.FindFirst(ClaimTypes.Email)?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Mensaje = "¡Acceso concedido a la MainAPI!",
                Correo = correo,
                Rol = rol
            });
        }
    }
}
