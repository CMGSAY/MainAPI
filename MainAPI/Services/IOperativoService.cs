using MainAPI.Models.DTOs;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public interface IOperativoService
    {
        Task<object> GetCiclosAsync();
        Task<object> PostCicloAsync(CicloEscolarDto dto);
        Task<object> PutCicloAsync(int id, CicloEscolarDto dto);
        Task<bool> DeleteCicloAsync(int id);
        Task<bool> HabilitarCicloAsync(int id);

        Task<object> GetJornadasAsync();
        Task<object> PostJornadaAsync(JornadaDto dto);
        Task<object> PutJornadaAsync(int id, JornadaDto dto);
        Task<bool> DeleteJornadaAsync(int id);
        Task<bool> HabilitarJornadaAsync(int id);

        Task<object> GetSeccionesAsync();
        Task<object> PostSeccionAsync(SeccionDto dto);
        Task<object> PutSeccionAsync(int id, SeccionDto dto);
        Task<bool> DeleteSeccionAsync(int id);
        Task<bool> HabilitarSeccionAsync(int id);

        Task<object> GetModulosAsync();
        Task<object> PostModuloAsync(ModuloEdificioDto dto);
        Task<object> PutModuloAsync(int id, ModuloEdificioDto dto);
        Task<bool> DeleteModuloAsync(int id);
        Task<bool> HabilitarModuloAsync(int id);

        Task<object> GetAulasAsync();
        Task<object> PostAulaAsync(AulaDto dto);
        Task<object> PutAulaAsync(int id, AulaDto dto);
        Task<bool> DeleteAulaAsync(int id);
        Task<bool> HabilitarAulaAsync(int id);

        Task<object> GetPrerrequisitosAsync();
        Task<object> PostPrerrequisitoAsync(CursoPrerrequisitoDto dto);
        Task<bool> DeletePrerrequisitoAsync(int id);
    }
}
