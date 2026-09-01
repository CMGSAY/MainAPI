using MainAPI.Models.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MainAPI.Services
{
    public interface IAsignacionesService
    {
        Task<object> GetCursosHabilitadosAsync();
        Task<object> GetHorariosOcupadosAsync(int idSeccion, int idAula, string dia);
        Task<(bool IsSuccess, string Message)> PostCursoHabilitadoAsync(CursoHabilitadoDto dto);
        Task<object> GetAsignacionesAsync();
        Task<object> PostAsignacionAsync(AsignacionCursoDto dto);
        Task<(bool IsSuccess, string Message)> PostMatriculaMultipleAsync(MatriculaMultipleDto dto);
        Task<object> GetCursosHabilitadosPorPensumAsync(int idCarreraSemestreCurso);
        Task<object> GetCursosActivosAsync();
        Task<(bool IsSuccess, string Message)> DesactivarCursoAsync(int idCursoHabilitado);
    }
}
