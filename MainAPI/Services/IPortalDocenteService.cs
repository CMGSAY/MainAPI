using MainAPI.Models.DTOs;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public interface IPortalDocenteService
    {
        Task<int> GetCatedraticoIdAsync(int loginUserId);
        Task<object> GetMisCursosAsync(int idCatedratico);
        Task<object> GetMaterialesAsync();
        Task<object> PostMaterialAsync(MaterialDto dto);
        Task<object> GetTareasAsync();
        Task<object> PostTareaAsync(TareaDto dto);
        Task<object> GetEntregasAsync();
        Task<(bool IsSuccess, object? Result)> CalificarEntregaAsync(int id, CalificacionTareaDto dto);
        Task<object> GetEvaluacionesAsync();
        Task<object> PostEvaluacionAsync(EvaluacionFijaDto dto);
        Task<object> PostNotaAsync(CalificacionEvaluacionDto dto);
    }
}
