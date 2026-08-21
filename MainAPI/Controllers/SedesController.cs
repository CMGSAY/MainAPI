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
    public class SedesController : ControllerBase
    {
        private readonly MainDbContext _context;
        public SedesController(MainDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _context.Sedes.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var e = await _context.Sedes.FindAsync(id);
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Post(SedeDto d)
        {
            var e = new Sede { Nombre = d.Nombre, UbicacionExacta = d.UbicacionExacta, Zona = d.Zona, IdMunicipio = d.IdMunicipio, TelefonoContactoPrincipal = d.TelefonoContactoPrincipal };
            _context.Sedes.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Put(int id, SedeDto d)
        {
            var e = await _context.Sedes.FindAsync(id);
            if (e == null) return NotFound();
            e.Nombre = d.Nombre; e.UbicacionExacta = d.UbicacionExacta; e.Zona = d.Zona; e.IdMunicipio = d.IdMunicipio; e.TelefonoContactoPrincipal = d.TelefonoContactoPrincipal;
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var e = await _context.Sedes.FindAsync(id);
            if (e == null) return NotFound();
            _context.Sedes.Remove(e);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}