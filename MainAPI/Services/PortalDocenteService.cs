using MainAPI.Data;
using MainAPI.Models;
using MainAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public class PortalDocenteService : IPortalDocenteService
    {
        private readonly MainDbContext _context;

        public PortalDocenteService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCatedraticoIdAsync(int loginUserId)
        {
            var catedratico = await _context.PerfilCatedraticos.FirstOrDefaultAsync(p => p.IdPersonaNavigation.LoginUserId == loginUserId);
            return catedratico?.IdCatedratico ?? 0;
        }

        public async Task<object> GetMisCursosAsync(int idCatedratico)
        {
            var cursos = await _context.CursoHabilitados
                .Include(c => c.IdCarreraSemestreCursoNavigation)
                    .ThenInclude(csc => csc.IdCursoNavigation)
                .Where(c => c.IdCatedratico == idCatedratico)
                .Select(c => new {
                    IdCursoHabilitado = c.IdCursoHabilitado,
                    Curso = c.IdCarreraSemestreCursoNavigation.IdCursoNavigation.NombreCurso,
                    Estado = c.Estado
                }).ToListAsync();

            return cursos;
        }

        public async Task<object> GetMaterialesAsync()
        {
            return await _context.MaterialClases.ToListAsync();
        }

        public async Task<object> PostMaterialAsync(MaterialDto d)
        {
            var e = new MaterialClase { IdCursoHabilitado = d.IdCursoHabilitado, Titulo = d.Titulo, Descripcion = d.Descripcion, TipoArchivo = d.TipoArchivo, UrlDocumento = d.UrlDocumento, FechaSubida = DateOnly.FromDateTime(DateTime.Now), Visibilidad = true };
            _context.MaterialClases.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<object> GetTareasAsync()
        {
            return await _context.Tareas.ToListAsync();
        }

        public async Task<object> PostTareaAsync(TareaDto d)
        {
            var e = new Tarea { IdCursoHabilitado = d.IdCursoHabilitado, Titulo = d.Titulo, Descripcion = d.Descripcion, UrlDocumentoReferencia = d.UrlDocumentoReferencia, FechaVencimiento = d.FechaVencimiento, PunteoMaximo = d.PunteoMaximo, FechaCreacion = DateOnly.FromDateTime(DateTime.Now), Visibilidad = true };
            _context.Tareas.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<object> GetEntregasAsync()
        {
            return await _context.EntregaTareas.ToListAsync();
        }

        public async Task<(bool IsSuccess, object? Result)> CalificarEntregaAsync(int id, CalificacionTareaDto d)
        {
            var e = await _context.EntregaTareas.FindAsync(id);
            if (e == null) return (false, null);

            e.Calificacion = d.Calificacion; 
            e.ComentariosCatedratico = d.Comentarios;
            await _context.SaveChangesAsync();
            return (true, e);
        }

        public async Task<object> GetEvaluacionesAsync()
        {
            return await _context.EvaluacionFijas.ToListAsync();
        }

        public async Task<object> PostEvaluacionAsync(EvaluacionFijaDto d)
        {
            var e = new EvaluacionFija { IdCursoHabilitado = d.IdCursoHabilitado, TipoEvaluacion = d.TipoEvaluacion, PunteoAsignado = d.PunteoAsignado, FechaEvaluacion = d.FechaEvaluacion };
            _context.EvaluacionFijas.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<object> PostNotaAsync(CalificacionEvaluacionDto d)
        {
            var e = new CalificacionEvaluacion { IdEvaluacion = d.IdEvaluacion, IdEstudiante = d.IdEstudiante, NotaObtenida = d.NotaObtenida };
            _context.CalificacionEvaluacions.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }
    }
}
