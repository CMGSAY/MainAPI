using MainAPI.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public interface IDocenteService
    {
        Task<int> GetDocenteIdAsync(int loginUserId);
        Task<object?> GetMisCursosAsync(int idCatedratico);
        Task<object?> GetSemanasCursoAsync(int idCursoHabilitado);
        Task<(bool IsSuccess, string Message, object? Tarea)> CrearTareaAsync(int idCursoHabilitado, CrearTareaDto dto);
        Task<object> CrearMaterialAsync(int idCursoHabilitado, CrearMaterialDto dto);
        Task<object> GetGradebookAsync(int idCursoHabilitado);
        Task<(bool IsSuccess, string Message, decimal? PuntosCalculados)> CalificarPorPorcentajeAsync(int idEntrega, CalificarDto dto);
        Task<(bool IsSuccess, string Message)> CalificarDirectoAsync(int idEntrega, CalificacionDto dto);
        Task<object> GetParticipantesAsync(int idCursoHabilitado);
        Task<object> GetEntregasPorTareaAsync(int idTarea);
        Task<(bool IsSuccess, string Message)> GenerarParcialesOficialesAsync(int idCursoHabilitado);
    }
}
