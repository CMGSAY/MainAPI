using MainAPI.Data;
using MainAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace MainAPI.Services
{
    public class CatalogosService : ICatalogosService
    {
        private readonly MainDbContext _context;

        public CatalogosService(MainDbContext context)
        {
            _context = context;
        }

        // DEPARTAMENTOS
        public async Task<object> GetDepartamentosAsync() => await _context.Departamentos.ToListAsync();
        
        public async Task<object> PostDepartamentoAsync(Departamento dto)
        {
            dto.Activo = true;
            _context.Departamentos.Add(dto);
            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<object> PutDepartamentoAsync(int id, Departamento dto)
        {
            var e = await _context.Departamentos.FindAsync(id);
            if (e == null) return null;
            e.NombreDepartamento = dto.NombreDepartamento;
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<bool> DeleteDepartamentoAsync(int id)
        {
            var e = await _context.Departamentos.FindAsync(id);
            if (e == null) return false;
            e.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HabilitarDepartamentoAsync(int id)
        {
            var e = await _context.Departamentos.FindAsync(id);
            if (e == null) return false;
            e.Activo = true;
            await _context.SaveChangesAsync();
            return true;
        }

        // MUNICIPIOS
        public async Task<object> GetMunicipiosAsync() => await _context.Municipios.ToListAsync();
        
        public async Task<object> PostMunicipioAsync(Municipio dto)
        {
            dto.Activo = true;
            _context.Municipios.Add(dto);
            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<object> PutMunicipioAsync(int id, Municipio dto)
        {
            var e = await _context.Municipios.FindAsync(id);
            if (e == null) return null;
            e.NombreMunicipio = dto.NombreMunicipio;
            e.IdDepartamento = dto.IdDepartamento;
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<bool> DeleteMunicipioAsync(int id)
        {
            var e = await _context.Municipios.FindAsync(id);
            if (e == null) return false;
            e.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> HabilitarMunicipioAsync(int id)
        {
            var e = await _context.Municipios.FindAsync(id);
            if (e == null) return false;
            e.Activo = true;
            await _context.SaveChangesAsync();
            return true;
        }

        // SEDES
        public async Task<object> GetSedesAsync() => await _context.Sedes.ToListAsync();
        
        public async Task<object> PostSedeAsync(Sede dto)
        {
            dto.Activo = true;
            _context.Sedes.Add(dto);
            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<object> PutSedeAsync(int id, Sede dto)
        {
            var e = await _context.Sedes.FindAsync(id);
            if (e == null) return null;
            e.Nombre = dto.Nombre;
            e.UbicacionExacta = dto.UbicacionExacta;
            e.IdMunicipio = dto.IdMunicipio;
            e.TelefonoContactoPrincipal = dto.TelefonoContactoPrincipal;
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<bool> DeleteSedeAsync(int id)
        {
            var e = await _context.Sedes.FindAsync(id);
            if (e == null) return false;
            e.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> HabilitarSedeAsync(int id)
        {
            var e = await _context.Sedes.FindAsync(id);
            if (e == null) return false;
            e.Activo = true;
            await _context.SaveChangesAsync();
            return true;
        }

        // FACULTADES
        public async Task<object> GetFacultadesAsync() => await _context.Facultads.ToListAsync();
        
        public async Task<object> PostFacultadAsync(Facultad dto)
        {
            dto.Activo = true;
            _context.Facultads.Add(dto);
            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<object> PutFacultadAsync(int id, Facultad dto)
        {
            var e = await _context.Facultads.FindAsync(id);
            if (e == null) return null;
            e.NombreFacultad = dto.NombreFacultad;
            e.Descripcion = dto.Descripcion;
            e.IdSede = dto.IdSede;
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<bool> DeleteFacultadAsync(int id)
        {
            var e = await _context.Facultads.FindAsync(id);
            if (e == null) return false;
            e.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> HabilitarFacultadAsync(int id)
        {
            var e = await _context.Facultads.FindAsync(id);
            if (e == null) return false;
            e.Activo = true;
            await _context.SaveChangesAsync();
            return true;
        }
        
        // SEMESTRES
        public async Task<object> GetSemestresAsync() => await _context.Semestres.ToListAsync();
        
        public async Task<object> PostSemestreAsync(Semestre dto)
        {
            dto.Activo = true;
            _context.Semestres.Add(dto);
            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<object> PutSemestreAsync(int id, Semestre dto)
        {
            var e = await _context.Semestres.FindAsync(id);
            if (e == null) return null;
            e.NumeroOrden = dto.NumeroOrden;
            e.NombreSemestre = dto.NombreSemestre;
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<bool> DeleteSemestreAsync(int id)
        {
            var e = await _context.Semestres.FindAsync(id);
            if (e == null) return false;
            e.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> HabilitarSemestreAsync(int id)
        {
            var e = await _context.Semestres.FindAsync(id);
            if (e == null) return false;
            e.Activo = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
