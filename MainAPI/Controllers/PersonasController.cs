using MainAPI.Data;
using MainAPI.Models;
using MainAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        [HttpGet("gestion-usuarios")]
        public async Task<IActionResult> GetGestionUsuarios()
        {
            var personas = await _context.Personas
                .Select(p => new
                {
                    IdPersona = p.IdPersona,
                    LoginUserId = p.LoginUserId,
                    NombreCompleto = p.PrimerNombre + " " + p.PrimerApellido,
                
                    EsAdmin = _context.PerfilAdministradors.Any(a => a.IdPersona == p.IdPersona),
                    EsDocente = _context.PerfilCatedraticos.Any(c => c.IdPersona == p.IdPersona),
                    EsEstudiante = _context.PerfilEstudiantes.Any(e => e.IdPersona == p.IdPersona)
                })
                .ToListAsync();

            var resultado = personas.Select(p => new
            {
                p.IdPersona,
                p.LoginUserId,
                p.NombreCompleto,
                RolPrincipal = p.EsAdmin ? "Administrador" : (p.EsDocente ? "Docente" : (p.EsEstudiante ? "Estudiante" : "Sin Rol"))
            }).ToList();

            return Ok(resultado);
        }
    }
}