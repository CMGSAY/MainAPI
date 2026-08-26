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
    public class PerfilesController : ControllerBase
    {
        private readonly MainDbContext _context;
        public PerfilesController(MainDbContext context) => _context = context;

        [HttpGet("estudiantes")]
        public async Task<IActionResult> GetEstudiantes() => Ok(await _context.PerfilEstudiantes.ToListAsync());

        [HttpPost("estudiantes")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PostEstudiante(PerfilEstudianteDto d)
        {
            var e = new PerfilEstudiante { IdPersona = d.IdPersona, Carnet = d.Carnet, TelefonoPrincipal = d.TelefonoPrincipal, DireccionCalleAvenida = d.DireccionCalleAvenida, Zona = d.Zona, IdMunicipio = d.IdMunicipio, FechaIngreso = d.FechaIngreso };
            _context.PerfilEstudiantes.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("catedraticos")]
        public async Task<IActionResult> GetCatedraticos() => Ok(await _context.PerfilCatedraticos.ToListAsync());

        [HttpPost("catedraticos")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PostCatedratico(PerfilCatedraticoDto d)
        {
            var e = new PerfilCatedratico { IdPersona = d.IdPersona, Dpi = d.Dpi, NumeroColegiadoActivo = d.NumeroColegiadoActivo, TelefonoPrincipal = d.TelefonoPrincipal, DireccionCalleAvenida = d.DireccionCalleAvenida, Zona = d.Zona, IdMunicipio = d.IdMunicipio, Especialidad = d.Especialidad, FechaContratacion = d.FechaContratacion };
            _context.PerfilCatedraticos.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("admins")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAdmins() => Ok(await _context.PerfilAdministradors.ToListAsync());

        [HttpPost("admins")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PostAdmin(PerfilAdministradorDto d)
        {
            var e = new PerfilAdministrador { IdPersona = d.IdPersona, Dpi = d.Dpi, NumeroColegiadoActivo = d.NumeroColegiadoActivo, TelefonoPrincipal = d.TelefonoPrincipal, DireccionCalleAvenida = d.DireccionCalleAvenida, Zona = d.Zona, IdMunicipio = d.IdMunicipio, Especialidad = d.Especialidad, FechaContratacion = d.FechaContratacion };
            _context.PerfilAdministradors.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("catedraticos/busqueda")]
        public async Task<IActionResult> GetAllCatedraticosBusqueda()
        {
            var res = await _context.PerfilCatedraticos
                .Include(p => p.IdPersonaNavigation)
                .Select(p => new {
                    IdCatedratico = p.IdCatedratico,
                    Dpi = p.Dpi,
                    DisplayString = p.IdPersonaNavigation.PrimerNombre + " " + p.IdPersonaNavigation.PrimerApellido
                })
                .ToListAsync();
            return Ok(res);
        }

        
        [HttpGet("estudiantes/municipio/{idMunicipio}")]
        public async Task<IActionResult> GetEstudiantesByMunicipio(int idMunicipio)
        {
            var res = await _context.PerfilEstudiantes
                .Include(p => p.IdPersonaNavigation)
                .Where(p => p.IdMunicipio == idMunicipio)
                .Select(p => new {
                    IdEstudiante = p.IdEstudiante,
                    DisplayString = (p.Carnet ?? p.IdEstudiante.ToString()) + " - " + p.IdPersonaNavigation.PrimerNombre + " " + p.IdPersonaNavigation.PrimerApellido
                })
                .ToListAsync();
            return Ok(res);
        }
        [HttpPut("estudiantes/{idEstudiante}/semestre/{idSemestre}")]
        public async Task<IActionResult> AsignarSemestre(int idEstudiante, int idSemestre)
        {
            var estudiante = await _context.PerfilEstudiantes.FindAsync(idEstudiante);
            if (estudiante == null) return NotFound("Estudiante no encontrado.");

            estudiante.IdSemestreActual = idSemestre;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Semestre asignado correctamente." });
        }

    }
}