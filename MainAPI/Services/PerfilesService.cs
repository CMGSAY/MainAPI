using MainAPI.Data;
using MainAPI.Models;
using MainAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static MainAPI.Controllers.PerfilesController;

namespace MainAPI.Services
{
    public class PerfilesService : IPerfilesService
    {
        private readonly MainDbContext _context;

        public PerfilesService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetEstudiantesAsync()
        {
            return await _context.PerfilEstudiantes.ToListAsync();
        }

        public async Task<object> GetAllEstudiantesBusquedaAsync()
        {
            var res = await _context.PerfilEstudiantes
                .Include(p => p.IdPersonaNavigation)
                .Select(p => new {
                    IdEstudiante = p.IdEstudiante,
                    Carnet = p.Carnet,
                    DisplayString = p.IdPersonaNavigation.PrimerNombre + " " + p.IdPersonaNavigation.PrimerApellido
                })
                .ToListAsync();
            return res;
        }

        public async Task<object> PostEstudianteAsync(PerfilEstudianteDto d)
        {
            var e = new PerfilEstudiante { IdPersona = d.IdPersona, Carnet = d.Carnet, TelefonoPrincipal = d.TelefonoPrincipal, DireccionCalleAvenida = d.DireccionCalleAvenida, Zona = d.Zona, IdMunicipio = d.IdMunicipio, FechaIngreso = d.FechaIngreso };
            _context.PerfilEstudiantes.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<object> GetCatedraticosAsync()
        {
            return await _context.PerfilCatedraticos.ToListAsync();
        }

        public async Task<object> PostCatedraticoAsync(PerfilCatedraticoDto d)
        {
            var e = new PerfilCatedratico { IdPersona = d.IdPersona, Dpi = d.Dpi, NumeroColegiadoActivo = d.NumeroColegiadoActivo, TelefonoPrincipal = d.TelefonoPrincipal, DireccionCalleAvenida = d.DireccionCalleAvenida, Zona = d.Zona, IdMunicipio = d.IdMunicipio, Especialidad = d.Especialidad, FechaContratacion = d.FechaContratacion };
            _context.PerfilCatedraticos.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<object> GetAdminsAsync()
        {
            return await _context.PerfilAdministradors.ToListAsync();
        }

        public async Task<object> PostAdminAsync(PerfilAdministradorDto d)
        {
            var e = new PerfilAdministrador { IdPersona = d.IdPersona, Dpi = d.Dpi, NumeroColegiadoActivo = d.NumeroColegiadoActivo, TelefonoPrincipal = d.TelefonoPrincipal, DireccionCalleAvenida = d.DireccionCalleAvenida, Zona = d.Zona, IdMunicipio = d.IdMunicipio, Especialidad = d.Especialidad, FechaContratacion = d.FechaContratacion };
            _context.PerfilAdministradors.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<object> GetAllCatedraticosBusquedaAsync()
        {
            var res = await _context.PerfilCatedraticos
                .Include(p => p.IdPersonaNavigation)
                .Select(p => new {
                    IdCatedratico = p.IdCatedratico,
                    Dpi = p.Dpi,
                    DisplayString = p.IdPersonaNavigation.PrimerNombre + " " + p.IdPersonaNavigation.PrimerApellido
                })
                .ToListAsync();
            return res;
        }

        public async Task<bool> ValidarDpiDuplicadoAsync(string dpi)
        {
            if (string.IsNullOrWhiteSpace(dpi)) return false;

            bool existeComoCatedratico = await _context.PerfilCatedraticos.AnyAsync(p => p.Dpi == dpi);
            bool existeComoAdmin = await _context.PerfilAdministradors.AnyAsync(p => p.Dpi == dpi);

            return existeComoCatedratico || existeComoAdmin;
        }

        public async Task<object> GetEstudiantesByMunicipioAsync(int idMunicipio)
        {
            var res = await _context.PerfilEstudiantes
                .Include(p => p.IdPersonaNavigation)
                .Where(p => p.IdMunicipio == idMunicipio)
                .Select(p => new {
                    IdEstudiante = p.IdEstudiante,
                    DisplayString = (p.Carnet ?? p.IdEstudiante.ToString()) + " - " + p.IdPersonaNavigation.PrimerNombre + " " + p.IdPersonaNavigation.PrimerApellido
                })
                .ToListAsync();
            return res;
        }

        public async Task<(bool IsSuccess, string Message)> AsignarSemestreAsync(int idEstudiante, int idSemestre)
        {
            var estudiante = await _context.PerfilEstudiantes.FindAsync(idEstudiante);
            if (estudiante == null) return (false, "Estudiante no encontrado.");

            estudiante.IdSemestreActual = idSemestre;
            await _context.SaveChangesAsync();

            return (true, "Semestre asignado correctamente.");
        }

        public async Task<(bool IsSuccess, string Message)> AsignarRutaAcademicaAsync(int idEstudiante, RutaAcademicaDto dto)
        {
            var estudiante = await _context.PerfilEstudiantes.FindAsync(idEstudiante);
            if (estudiante == null) return (false, "Estudiante no encontrado.");

            estudiante.IdCarrera = dto.IdCarrera;
            estudiante.IdSemestreActual = dto.IdSemestre;

            await _context.SaveChangesAsync();

            return (true, "Ruta académica asignada correctamente.");
        }
    }
}
