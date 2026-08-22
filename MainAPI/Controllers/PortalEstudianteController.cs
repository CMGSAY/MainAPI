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
    [Authorize(Roles = "Estudiante")]
    public class PortalEstudianteController : ControllerBase
    {
        private readonly MainDbContext _context;
        public PortalEstudianteController(MainDbContext context) => _context = context;

        [HttpGet("mis-cursos")]
        public async Task<IActionResult> GetMisCursos() => Ok(await _context.AsignacionCursos.ToListAsync());

        [HttpGet("mis-materiales")]
        public async Task<IActionResult> GetMisMateriales() => Ok(await _context.MaterialClases.ToListAsync());

        [HttpGet("mis-tareas")]
        public async Task<IActionResult> GetMisTareas() => Ok(await _context.Tareas.ToListAsync());

        [HttpPost("entregas")]
        public async Task<IActionResult> PostEntrega(EntregaTareaDto d)
        {
            var e = new EntregaTarea { IdTarea = d.IdTarea, IdEstudiante = d.IdEstudiante, UrlArchivoAdjunto = d.UrlArchivoAdjunto, FechaEnvio = DateTime.Now };
            _context.EntregaTareas.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("mis-notas")]
        public async Task<IActionResult> GetMisNotas() => Ok(await _context.CalificacionEvaluacions.ToListAsync());

        [HttpGet("kardex")]
        public async Task<IActionResult> GetKardex()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized("Token inválido.");
            int userId = int.Parse(userIdString);

            var estudiante = await _context.PerfilEstudiantes.FirstOrDefaultAsync(p => p.IdPersonaNavigation.LoginUserId == userId);
            if (estudiante == null) return Unauthorized("Perfil no encontrado.");

            var historial = await _context.AsignacionCursos
                .Include(a => a.IdCursoHabilitadoNavigation.IdCarreraSemestreCursoNavigation.IdCursoNavigation)
                .Where(a => a.IdEstudiante == estudiante.IdEstudiante)
                .Select(a => new {
                    Curso = a.IdCursoHabilitadoNavigation.IdCarreraSemestreCursoNavigation.IdCursoNavigation.NombreCurso,
                    NotaFinal = a.NotaFinal,
                    Estado = a.NotaFinal >= 61 ? "Aprobado" : "Reprobado"
                }).ToListAsync();

            return Ok(historial);
        }
    }
}