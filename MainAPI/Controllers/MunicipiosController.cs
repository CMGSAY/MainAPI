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
    public class MunicipiosController : ControllerBase
    {
        private readonly MainDbContext _context;
        public MunicipiosController(MainDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _context.Municipios.ToListAsync());

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Post(MunicipioDto d)
        {
            var e = new Municipio { IdDepartamento = d.IdDepartamento, NombreMunicipio = d.NombreMunicipio };
            _context.Municipios.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }
    }
}