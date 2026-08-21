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
            var e = new Curso { NombreCurso = d.NombreCurso, Creditos = d.Creditos, Descripcion = d.Descripcion, PunteoMaximoTotal = d.PunteoMaximoTotal ?? 100 };
            _context.Cursos.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }
    }
}