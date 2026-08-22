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
    public class SemestresController : ControllerBase
    {
        private readonly MainDbContext _context;
        public SemestresController(MainDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _context.Semestres.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Post(SemestreDto d)
        {
            var e = new Semestre { NombreSemestre = d.NombreSemestre, NumeroOrden = d.NumeroOrden };
            _context.Semestres.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }
    }
}