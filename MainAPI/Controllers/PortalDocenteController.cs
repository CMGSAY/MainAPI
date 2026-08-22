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
    [Authorize(Roles = "Docente,Administrador")]
    public class PortalDocenteController : ControllerBase
    {
        private readonly MainDbContext _context;
        public PortalDocenteController(MainDbContext context) => _context = context;

        [HttpGet("mis-cursos")]
        public async Task<IActionResult> GetMisCursos()
        {
            // Extraemos el ID de usuario desde el token JWT
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized("Token inválido.");
            int userId = int.Parse(userIdString);

            // Buscamos el perfil del catedrático
            var catedratico = await _context.PerfilCatedraticos.FirstOrDefaultAsync(p => p.IdPersonaNavigation.LoginUserId == userId);
            if (catedratico == null) return Unauthorized("Perfil de catedrático no encontrado.");

            var cursos = await _context.CursoHabilitados
                .Include(c => c.IdCarreraSemestreCursoNavigation)
                    .ThenInclude(csc => csc.IdCursoNavigation)
                .Where(c => c.IdCatedratico == catedratico.IdCatedratico)
                .Select(c => new {
                    IdCursoHabilitado = c.IdCursoHabilitado,
                    Curso = c.IdCarreraSemestreCursoNavigation.IdCursoNavigation.NombreCurso,
                    Estado = c.Estado
                }).ToListAsync();

            return Ok(cursos);
        }

        [HttpGet("materiales")]
        public async Task<IActionResult> GetMateriales() => Ok(await _context.MaterialClases.ToListAsync());

        [HttpPost("materiales")]
        public async Task<IActionResult> PostMaterial(MaterialDto d)
        {
            var e = new MaterialClase { IdCursoHabilitado = d.IdCursoHabilitado, Titulo = d.Titulo, Descripcion = d.Descripcion, TipoArchivo = d.TipoArchivo, UrlDocumento = d.UrlDocumento, FechaSubida = DateOnly.FromDateTime(DateTime.Now), Visibilidad = true };
            _context.MaterialClases.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("tareas")]
        public async Task<IActionResult> GetTareas() => Ok(await _context.Tareas.ToListAsync());

        [HttpPost("tareas")]
        public async Task<IActionResult> PostTarea(TareaDto d)
        {
            var e = new Tarea { IdCursoHabilitado = d.IdCursoHabilitado, Titulo = d.Titulo, Descripcion = d.Descripcion, UrlDocumentoReferencia = d.UrlDocumentoReferencia, FechaVencimiento = d.FechaVencimiento, PunteoMaximo = d.PunteoMaximo, FechaCreacion = DateOnly.FromDateTime(DateTime.Now), Visibilidad = true };
            _context.Tareas.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("entregas")]
        public async Task<IActionResult> GetEntregas() => Ok(await _context.EntregaTareas.ToListAsync());

        [HttpPut("entregas/{id}/calificar")]
        public async Task<IActionResult> CalificarEntrega(int id, CalificacionTareaDto d)
        {
            var e = await _context.EntregaTareas.FindAsync(id);
            if (e == null) return NotFound();
            e.Calificacion = d.Calificacion; e.ComentariosCatedratico = d.Comentarios;
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("evaluaciones")]
        public async Task<IActionResult> GetEvaluaciones() => Ok(await _context.EvaluacionFijas.ToListAsync());

        [HttpPost("evaluaciones")]
        public async Task<IActionResult> PostEvaluacion(EvaluacionFijaDto d)
        {
            var e = new EvaluacionFija { IdCursoHabilitado = d.IdCursoHabilitado, TipoEvaluacion = d.TipoEvaluacion, PunteoAsignado = d.PunteoAsignado, FechaEvaluacion = d.FechaEvaluacion };
            _context.EvaluacionFijas.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpPost("evaluaciones/notas")]
        public async Task<IActionResult> PostNota(CalificacionEvaluacionDto d)
        {
            var e = new CalificacionEvaluacion { IdEvaluacion = d.IdEvaluacion, IdEstudiante = d.IdEstudiante, NotaObtenida = d.NotaObtenida };
            _context.CalificacionEvaluacions.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }
    }
}