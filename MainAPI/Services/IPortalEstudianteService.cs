using MainAPI.Models.DTOs;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public interface IPortalEstudianteService
    {
        Task<int> GetEstudianteIdAsync(int loginUserId);
        Task<object?> GetMisCursosAsync(int idEstudiante);
        Task<(bool IsSuccess, string Message, object? Semanas)> GetSemanasCursoAsync(int idCursoHabilitado);
        Task<(bool IsSuccess, string Message, int? IdEntrega)> PostEntregaAsync(int idEstudiante, EntregaTareaDto dto);
        Task<object?> GetKardexAsync(int idEstudiante);
        Task<(bool IsSuccess, string Message, object? Cursos)> GetCursosDisponiblesMatriculaAsync(int idEstudiante);
        Task<(bool IsSuccess, string Message)> MatricularseAsync(int idEstudiante, int idCursoHabilitado);
    }
}
