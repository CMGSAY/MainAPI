using MainAPI.Models;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public interface ICatalogosService
    {
        // Departamentos
        Task<object> GetDepartamentosAsync();
        Task<object> PostDepartamentoAsync(Departamento dto);
        Task<object> PutDepartamentoAsync(int id, Departamento dto);
        Task<bool> DeleteDepartamentoAsync(int id);
        Task<bool> HabilitarDepartamentoAsync(int id);

        // Municipios
        Task<object> GetMunicipiosAsync();
        Task<object> PostMunicipioAsync(Municipio dto);
        Task<object> PutMunicipioAsync(int id, Municipio dto);
        Task<bool> DeleteMunicipioAsync(int id);
        Task<bool> HabilitarMunicipioAsync(int id);

        // Sedes
        Task<object> GetSedesAsync();
        Task<object> PostSedeAsync(Sede dto);
        Task<object> PutSedeAsync(int id, Sede dto);
        Task<bool> DeleteSedeAsync(int id);
        Task<bool> HabilitarSedeAsync(int id);

        // Facultades
        Task<object> GetFacultadesAsync();
        Task<object> PostFacultadAsync(Facultad dto);
        Task<object> PutFacultadAsync(int id, Facultad dto);
        Task<bool> DeleteFacultadAsync(int id);
        Task<bool> HabilitarFacultadAsync(int id);
        
        // Semestres
        Task<object> GetSemestresAsync();
        Task<object> PostSemestreAsync(Semestre dto);
        Task<object> PutSemestreAsync(int id, Semestre dto);
        Task<bool> DeleteSemestreAsync(int id);
        Task<bool> HabilitarSemestreAsync(int id);
    }
}
