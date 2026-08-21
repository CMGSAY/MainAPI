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
    public class RegistrosController : ControllerBase
    {
        private readonly MainDbContext _context;
        public RegistrosController(MainDbContext context) => _context = context;

        [HttpGet("sesiones")]
        public async Task<IActionResult> GetSesiones() => Ok(await _context.ClaseSesions.ToListAsync());

        [HttpPost("sesiones")]
        [Authorize(Roles = "Docente,Administrador")]
        public async Task<IActionResult> PostSesion(SesionDto d)
        {
            var e = new ClaseSesion { IdCursoHabilitado = d.IdCursoHabilitado, FechaSesion = d.FechaSesion, HoraInicio = d.HoraInicio, HoraFin = d.HoraFin, TemaImpartido = d.TemaImpartido };
            _context.ClaseSesions.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("asistencias")]
        public async Task<IActionResult> GetAsistencias() => Ok(await _context.AsistenciaEstudiantes.ToListAsync());

        [HttpPost("asistencias")]
        [Authorize(Roles = "Docente,Administrador")]
        public async Task<IActionResult> PostAsistencia(AsistenciaDto d)
        {
            var e = new AsistenciaEstudiante { IdSesion = d.IdSesion, IdEstudiante = d.IdEstudiante, EstadoAsistencia = d.EstadoAsistencia, FechaRegistro = DateTime.Now };
            _context.AsistenciaEstudiantes.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("excusas")]
        public async Task<IActionResult> GetExcusas() => Ok(await _context.ExcusaInasistencia.ToListAsync());

        [HttpPost("excusas")]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> PostExcusa(ExcusaDto d)
        {
            var e = new ExcusaInasistencium { IdAsistenciaEst = d.IdAsistenciaEst, Motivo = d.Motivo, UrlComprobante = d.UrlComprobante, EstadoAprobacion = "Pendiente", FechaSolicitud = DateTime.Now };
            _context.ExcusaInasistencia.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpPut("excusas/{id}/aprobar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AprobarExcusa(int id, [FromBody] string estado)
        {
            var e = await _context.ExcusaInasistencia.FindAsync(id);
            if (e == null) return NotFound();
            e.EstadoAprobacion = estado;
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("bitacora")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetBitacora() => Ok(await _context.Bitacoras.ToListAsync());

        [HttpPost("bitacora")]
        public async Task<IActionResult> PostBitacora(BitacoraDto d)
        {
            var e = new Bitacora { LoginUserId = d.LoginUserId, Accion = d.Accion, Ip = d.Ip, FechaHora = DateTime.Now };
            _context.Bitacoras.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }
    }
}