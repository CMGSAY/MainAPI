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
    public class DepartamentosController : ControllerBase
    {
        private readonly MainDbContext _context;
        public DepartamentosController(MainDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _context.Departamentos.ToListAsync());

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Post(DepartamentoDto d)
        {
            var e = new Departamento { NombreDepartamento = d.NombreDepartamento };
            _context.Departamentos.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }
    }
}