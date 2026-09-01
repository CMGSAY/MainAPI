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
    public class CursosController : ControllerBase
    {
        private readonly MainDbContext _context;
        public CursosController(MainDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _context.Cursos.ToListAsync());

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Post(CursoDto d)
        {
            var e = new Curso { NombreCurso = d.NombreCurso, Creditos = d.Creditos, Descripcion = d.Descripcion, PunteoMaximoTotal = d.PunteoMaximoTotal ?? 100, Activo = true };
            _context.Cursos.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Put(int id, CursoDto d)
        {
            var e = await _context.Cursos.FindAsync(id);
            if (e == null) return NotFound();
            e.NombreCurso = d.NombreCurso ?? e.NombreCurso;
            e.Descripcion = d.Descripcion ?? e.Descripcion;
            if (d.Creditos > 0) e.Creditos = d.Creditos;
            if (d.PunteoMaximoTotal > 0) e.PunteoMaximoTotal = d.PunteoMaximoTotal ?? 100;
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var e = await _context.Cursos.FindAsync(id);
            if (e == null) return NotFound();
            e.Activo = false;
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Deshabilitado" });
        }

        [HttpPut("{id}/habilitar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Habilitar(int id)
        {
            var e = await _context.Cursos.FindAsync(id);
            if (e == null) return NotFound();
            e.Activo = true;
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Habilitado" });
        }
    }
}