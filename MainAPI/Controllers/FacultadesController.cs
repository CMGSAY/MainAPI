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
    public class FacultadesController : ControllerBase
    {
        private readonly MainDbContext _context;
        public FacultadesController(MainDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _context.Facultads.ToListAsync());

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Post(FacultadDto d)
        {
            var e = new Facultad { NombreFacultad = d.NombreFacultad, Descripcion = d.Descripcion, IdSede = d.IdSede };
            _context.Facultads.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var e = await _context.Facultads.FindAsync(id);
            if (e == null) return NotFound();
            _context.Facultads.Remove(e);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}