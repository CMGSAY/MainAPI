using MainAPI.Data;
using MainAPI.Models;
using MainAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PensumController : ControllerBase
    {
        private readonly MainDbContext _context;

        public PensumController(MainDbContext context)
        {
            _context = context;
        }

        [HttpPost("Vincular")]
        public async Task<IActionResult> VincularCurso(VincularCursoPensumDto dto)
        {

            var carreraSemestre = await _context.CarreraSemestres
                .FirstOrDefaultAsync(cs => cs.IdCarrera == dto.IdCarrera && cs.IdSemestre == dto.IdSemestre);

            if (carreraSemestre == null)
            {
                carreraSemestre = new CarreraSemestre
                {
                    IdCarrera = dto.IdCarrera,
                    IdSemestre = dto.IdSemestre
                };
                _context.CarreraSemestres.Add(carreraSemestre);
                await _context.SaveChangesAsync(); // Guardamos para que genere el IdCarreraSemestre
            }

            var vinculoExistente = await _context.CarreraSemestreCursos
                .FirstOrDefaultAsync(csc => csc.IdCarreraSemestre == carreraSemestre.IdCarreraSemestre && csc.IdCurso == dto.IdCurso);

            if (vinculoExistente != null)
            {
                return BadRequest("El curso ya está vinculado a este semestre y carrera.");
            }

            var nuevoVinculo = new CarreraSemestreCurso
            {
                IdCarreraSemestre = carreraSemestre.IdCarreraSemestre,
                IdCurso = dto.IdCurso
            };

            _context.CarreraSemestreCursos.Add(nuevoVinculo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Curso vinculado exitosamente al pensum." });
        }


        [HttpPost("CarreraSemestre")]
        public async Task<IActionResult> CrearCarreraSemestre(CarreraSemestreDto dto)
        {
            var carreraSemestre = new CarreraSemestre
            {
                IdCarrera = dto.IdCarrera,
                IdSemestre = dto.IdSemestre
            };

            _context.CarreraSemestres.Add(carreraSemestre);
            await _context.SaveChangesAsync();

            return Ok(carreraSemestre);
        }

        [HttpPost("CarreraSemestreCurso")]
        public async Task<IActionResult> CrearCarreraSemestreCurso(CarreraSemestreCursoDto dto)
        {
            var vinculo = new CarreraSemestreCurso
            {
                IdCarreraSemestre = dto.IdCarreraSemestre,
                IdCurso = dto.IdCurso
            };

            _context.CarreraSemestreCursos.Add(vinculo);
            await _context.SaveChangesAsync();

            return Ok(vinculo);
        }
    }
}
