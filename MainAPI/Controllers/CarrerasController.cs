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
    public class CarrerasController : ControllerBase
    {
        private readonly MainDbContext _context;
        public CarrerasController(MainDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _context.Carreras.ToListAsync());

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Post(CarreraDto d)
        {
            var e = new Carrera { NombreCarrera = d.NombreCarrera, Descripcion = d.Descripcion, IdFacultad = d.IdFacultad, CantidadSemestres = d.CantidadSemestres, CreditosTotales = d.CreditosTotales, Activa = d.Activa ?? true };
            _context.Carreras.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpPost("{idCarrera}/semestres/{idSemestre}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AsignarSemestre(int idCarrera, int idSemestre)
        {
            if (await _context.CarreraSemestres.AnyAsync(cs => cs.IdCarrera == idCarrera && cs.IdSemestre == idSemestre))
                return BadRequest(new { Mensaje = "Ya existe esta asignación." });
            var e = new CarreraSemestre { IdCarrera = idCarrera, IdSemestre = idSemestre };
            _context.CarreraSemestres.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpPost("carrera-semestre/{idCarreraSemestre}/cursos/{idCurso}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AsignarCurso(int idCarreraSemestre, int idCurso)
        {
            if (await _context.CarreraSemestreCursos.AnyAsync(csc => csc.IdCarreraSemestre == idCarreraSemestre && csc.IdCurso == idCurso))
                return BadRequest(new { Mensaje = "Ya existe esta asignación." });
            var e = new CarreraSemestreCurso { IdCarreraSemestre = idCarreraSemestre, IdCurso = idCurso };
            _context.CarreraSemestreCursos.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }
    }
}