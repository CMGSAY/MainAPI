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

        [HttpPost("vincular-curso-pensum")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> VincularCursoAPensum([FromBody] VincularPensumDto d)
        {
        
            var carreraSemestre = await _context.CarreraSemestres
                .FirstOrDefaultAsync(cs => cs.IdCarrera == d.IdCarrera && cs.IdSemestre == d.IdSemestre);

            if (carreraSemestre == null)
            {
                carreraSemestre = new CarreraSemestre { IdCarrera = d.IdCarrera, IdSemestre = d.IdSemestre };
                _context.CarreraSemestres.Add(carreraSemestre);
                await _context.SaveChangesAsync(); 
            }

            if (await _context.CarreraSemestreCursos.AnyAsync(csc => csc.IdCarreraSemestre == carreraSemestre.IdCarreraSemestre && csc.IdCurso == d.IdCurso))
            {
                return BadRequest(new { Mensaje = "Este curso ya está asignado a esta carrera en este semestre." });
            }

            var asignacion = new CarreraSemestreCurso { IdCarreraSemestre = carreraSemestre.IdCarreraSemestre, IdCurso = d.IdCurso };
            _context.CarreraSemestreCursos.Add(asignacion);
            await _context.SaveChangesAsync();

            return Ok(new { Mensaje = "Curso asignado exitosamente al pensum." });
        }

        [HttpGet("{idCarrera}/semestres/{idSemestre}/cursos")]
        public async Task<IActionResult> GetCursosPorCarreraYSemestre(int idCarrera, int idSemestre)
        {
            var cursos = await (from csc in _context.CarreraSemestreCursos
                                join cs in _context.CarreraSemestres on csc.IdCarreraSemestre equals cs.IdCarreraSemestre
                                join c in _context.Cursos on csc.IdCurso equals c.IdCurso
                                where cs.IdCarrera == idCarrera && cs.IdSemestre == idSemestre
                                select new
                                {
                                    IdCarreraSemestreCurso = csc.IdCarreraSemestreCurso,
                                    NombreCurso = c.NombreCurso
                                }).ToListAsync();

            return Ok(cursos);
        }
    }
}