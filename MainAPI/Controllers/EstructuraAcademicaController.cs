using MainAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EstructuraAcademicaController : ControllerBase
    {
        private readonly MainDbContext _context;
        public EstructuraAcademicaController(MainDbContext context) => _context = context;

        [HttpGet("departamentos")]
        public async Task<IActionResult> GetDepartamentos() => Ok(await _context.Departamentos.ToListAsync());

        [HttpGet("municipios/departamento/{idDepartamento}")]
        public async Task<IActionResult> GetMunicipios(int idDepartamento) =>
            Ok(await _context.Municipios.Where(m => m.IdDepartamento == idDepartamento).ToListAsync());

        [HttpGet("sedes/municipio/{idMunicipio}")]
        public async Task<IActionResult> GetSedes(int idMunicipio) =>
            Ok(await _context.Sedes.Where(s => s.IdMunicipio == idMunicipio).ToListAsync());

        [HttpGet("facultades/sede/{idSede}")]
        public async Task<IActionResult> GetFacultades(int idSede) =>
            Ok(await _context.Facultads.Where(f => f.IdSede == idSede).ToListAsync());

        [HttpGet("carreras/facultad/{idFacultad}")]
        public async Task<IActionResult> GetCarreras(int idFacultad) =>
            Ok(await _context.Carreras.Where(c => c.IdFacultad == idFacultad).ToListAsync());

        [HttpGet("semestres/carrera/{idCarrera}")]
        public async Task<IActionResult> GetSemestres(int idCarrera)
        {
            var semestres = await (from cs in _context.CarreraSemestres
                                   join s in _context.Semestres on cs.IdSemestre equals s.IdSemestre
                                   where cs.IdCarrera == idCarrera
                                   select s).ToListAsync();
            return Ok(semestres);
        }
    }
}