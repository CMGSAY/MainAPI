using MainAPI.Models.DTOs;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public interface IPersonasService
    {
        Task<object> GetPersonasAsync();
        Task<object> PostPersonaAsync(PersonaDto dto);
        Task<object> GetGestionUsuariosAsync();
        Task<(bool IsSuccess, string Message)> DeshabilitarPersonaAsync(int id);
        Task<(bool IsSuccess, string Message)> HabilitarPersonaAsync(int id);
    }
}
