using MainAPI.Data;
using MainAPI.Models;
using MainAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class PersonasController : ControllerBase
    {
        private readonly MainDbContext _context;
        public PersonasController(MainDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _context.Personas.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Post(PersonaDto d)
        {
            var e = new Persona { LoginUserId = d.LoginUserId, PrimerNombre = d.PrimerNombre, SegundoNombre = d.SegundoNombre, TercerNombre = d.TercerNombre, PrimerApellido = d.PrimerApellido, SegundoApellido = d.SegundoApellido };
            _context.Personas.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }
    }
}