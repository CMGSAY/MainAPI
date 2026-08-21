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
    public class OperativoController : ControllerBase
    {
        private readonly MainDbContext _context;
        public OperativoController(MainDbContext context) => _context = context;

        [HttpGet("ciclos")]
        public async Task<IActionResult> GetCiclos() => Ok(await _context.CicloEscolars.ToListAsync());

        [HttpPost("ciclos")]
        public async Task<IActionResult> PostCiclo(CicloEscolarDto d)
        {
            var e = new CicloEscolar { Anio = d.Anio, NombreCiclo = d.NombreCiclo, FechaInicio = d.FechaInicio, FechaFinalizacion = d.FechaFinalizacion, Estado = d.Estado ?? true };
            _context.CicloEscolars.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("jornada")]
        public async Task<IActionResult> GetJornadas() => Ok(await _context.Jornada.ToListAsync());

        [HttpPost("jornada")]
        public async Task<IActionResult> PostJornada(JornadaDto d)
        {
            var e = new Jornadum { NombreJornada = d.NombreJornada, HoraInicio = d.HoraInicio, HoraFin = d.HoraFin };
            _context.Jornada.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("secciones")]
        public async Task<IActionResult> GetSecciones() => Ok(await _context.Seccions.ToListAsync());

        [HttpPost("secciones")]
        public async Task<IActionResult> PostSeccion(SeccionDto d)
        {
            var e = new Seccion { NombreSeccion = d.NombreSeccion, CupoMaximo = d.CupoMaximo };
            _context.Seccions.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("modulos")]
        public async Task<IActionResult> GetModulos() => Ok(await _context.ModuloEdificios.ToListAsync());

        [HttpPost("modulos")]
        public async Task<IActionResult> PostModulo(ModuloEdificioDto d)
        {
            var e = new ModuloEdificio { Nombre = d.Nombre, Ubicacion = d.Ubicacion, IdSede = d.IdSede };
            _context.ModuloEdificios.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("aulas")]
        public async Task<IActionResult> GetAulas() => Ok(await _context.Aulas.ToListAsync());

        [HttpPost("aulas")]
        public async Task<IActionResult> PostAula(AulaDto d)
        {
            var e = new Aula { NumeroSalon = d.NumeroSalon, IdModulo = d.IdModulo, CapacidadAlumnos = d.CapacidadAlumnos };
            _context.Aulas.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("prerrequisitos")]
        public async Task<IActionResult> GetPrerreq() => Ok(await _context.CursoPrerrequisitos.ToListAsync());

        [HttpPost("prerrequisitos")]
        public async Task<IActionResult> PostPrerreq(CursoPrerrequisitoDto d)
        {
            var e = new CursoPrerrequisito { IdCurso = d.IdCurso, IdCursoRequerido = d.IdCursoRequerido };
            _context.CursoPrerrequisitos.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }
    }
}