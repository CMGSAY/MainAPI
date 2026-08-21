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
    [Authorize]
    public class PerfilesController : ControllerBase
    {
        private readonly MainDbContext _context;
        public PerfilesController(MainDbContext context) => _context = context;

        [HttpGet("estudiantes")]
        public async Task<IActionResult> GetEstudiantes() => Ok(await _context.PerfilEstudiantes.ToListAsync());

        [HttpPost("estudiantes")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PostEstudiante(PerfilEstudianteDto d)
        {
            var e = new PerfilEstudiante { IdPersona = d.IdPersona, Carnet = d.Carnet, TelefonoPrincipal = d.TelefonoPrincipal, DireccionCalleAvenida = d.DireccionCalleAvenida, Zona = d.Zona, IdMunicipio = d.IdMunicipio, FechaIngreso = d.FechaIngreso };
            _context.PerfilEstudiantes.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("catedraticos")]
        public async Task<IActionResult> GetCatedraticos() => Ok(await _context.PerfilCatedraticos.ToListAsync());

        [HttpPost("catedraticos")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PostCatedratico(PerfilCatedraticoDto d)
        {
            var e = new PerfilCatedratico { IdPersona = d.IdPersona, Dpi = d.Dpi, NumeroColegiadoActivo = d.NumeroColegiadoActivo, TelefonoPrincipal = d.TelefonoPrincipal, DireccionCalleAvenida = d.DireccionCalleAvenida, Zona = d.Zona, IdMunicipio = d.IdMunicipio, Especialidad = d.Especialidad, FechaContratacion = d.FechaContratacion };
            _context.PerfilCatedraticos.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("admins")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAdmins() => Ok(await _context.PerfilAdministradors.ToListAsync());

        [HttpPost("admins")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PostAdmin(PerfilAdministradorDto d)
        {
            var e = new PerfilAdministrador { IdPersona = d.IdPersona, Dpi = d.Dpi, NumeroColegiadoActivo = d.NumeroColegiadoActivo, TelefonoPrincipal = d.TelefonoPrincipal, DireccionCalleAvenida = d.DireccionCalleAvenida, Zona = d.Zona, IdMunicipio = d.IdMunicipio, Especialidad = d.Especialidad, FechaContratacion = d.FechaContratacion };
            _context.PerfilAdministradors.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }
    }
}