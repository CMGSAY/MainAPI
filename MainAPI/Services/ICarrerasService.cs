using MainAPI.Models.DTOs;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public interface ICarrerasService
    {
        Task<object> GetCarrerasAsync();
        Task<object> PostCarreraAsync(CarreraDto dto);
        Task<object> PutCarreraAsync(int id, CarreraDto dto);
        Task<bool> DeleteCarreraAsync(int id);
        Task<bool> HabilitarCarreraAsync(int id);
        Task<(bool IsSuccess, string Message, object? Result)> AsignarSemestreAsync(int idCarrera, int idSemestre);
        Task<(bool IsSuccess, string Message, object? Result)> AsignarCursoAsync(int idCarreraSemestre, int idCurso);
        Task<(bool IsSuccess, string Message)> VincularCursoAPensumAsync(VincularPensumDto dto);
        Task<object> GetCursosPorCarreraYSemestreAsync(int idCarrera, int idSemestre);
    }
}
