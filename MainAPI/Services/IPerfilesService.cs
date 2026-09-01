using MainAPI.Models.DTOs;
using System.Threading.Tasks;
using static MainAPI.Controllers.PerfilesController;

namespace MainAPI.Services
{
    public interface IPerfilesService
    {
        Task<object> GetEstudiantesAsync();
        Task<object> GetAllEstudiantesBusquedaAsync();
        Task<object> PostEstudianteAsync(PerfilEstudianteDto dto);
        Task<object> GetCatedraticosAsync();
        Task<object> PostCatedraticoAsync(PerfilCatedraticoDto dto);
        Task<object> GetAdminsAsync();
        Task<object> PostAdminAsync(PerfilAdministradorDto dto);
        Task<object> GetAllCatedraticosBusquedaAsync();
        Task<bool> ValidarDpiDuplicadoAsync(string dpi);
        Task<object> GetEstudiantesByMunicipioAsync(int idMunicipio);
        Task<(bool IsSuccess, string Message)> AsignarSemestreAsync(int idEstudiante, int idSemestre);
        Task<(bool IsSuccess, string Message)> AsignarRutaAcademicaAsync(int idEstudiante, RutaAcademicaDto dto);
    }
}
