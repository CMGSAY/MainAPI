using MainAPI.Data;
using MainAPI.Models;
using MainAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public class OperativoService : IOperativoService
    {
        private readonly MainDbContext _context;

        public OperativoService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetCiclosAsync()
        {
            return await _context.CicloEscolars.ToListAsync();
        }

        public async Task<object> PostCicloAsync(CicloEscolarDto d)
        {
            var e = new CicloEscolar { Anio = d.Anio, NombreCiclo = d.NombreCiclo, FechaInicio = d.FechaInicio, FechaFinalizacion = d.FechaFinalizacion, Estado = d.Estado ?? true };
            _context.CicloEscolars.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<object> PutCicloAsync(int id, CicloEscolarDto d)
        {
            var e = await _context.CicloEscolars.FindAsync(id);
            if (e == null) return null;
            e.NombreCiclo = d.NombreCiclo ?? e.NombreCiclo;
            if (d.Anio != 0) e.Anio = d.Anio;
            if (d.FechaInicio != default) e.FechaInicio = d.FechaInicio;
            if (d.FechaFinalizacion != default) e.FechaFinalizacion = d.FechaFinalizacion;
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<bool> DeleteCicloAsync(int id)
        {
            var e = await _context.CicloEscolars.FindAsync(id);
            if (e == null) return false;
            e.Estado = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HabilitarCicloAsync(int id)
        {
            var e = await _context.CicloEscolars.FindAsync(id);
            if (e == null) return false;
            e.Estado = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetJornadasAsync()
        {
            return await _context.Jornada.ToListAsync();
        }

        public async Task<object> PostJornadaAsync(JornadaDto d)
        {
            var e = new Jornadum { NombreJornada = d.NombreJornada, HoraInicio = d.HoraInicio, HoraFin = d.HoraFin, Activo = true };
            _context.Jornada.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<object> PutJornadaAsync(int id, JornadaDto d)
        {
            var e = await _context.Jornada.FindAsync(id);
            if (e == null) return null;
            e.NombreJornada = d.NombreJornada ?? e.NombreJornada;
            if (d.HoraInicio != default) e.HoraInicio = d.HoraInicio;
            if (d.HoraFin != default) e.HoraFin = d.HoraFin;
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<bool> DeleteJornadaAsync(int id)
        {
            var e = await _context.Jornada.FindAsync(id);
            if (e == null) return false;
            e.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HabilitarJornadaAsync(int id)
        {
            var e = await _context.Jornada.FindAsync(id);
            if (e == null) return false;
            e.Activo = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetSeccionesAsync()
        {
            return await _context.Seccions.ToListAsync();
        }

        public async Task<object> PostSeccionAsync(SeccionDto d)
        {
            var e = new Seccion
            {
                NombreSeccion = d.NombreSeccion,
                CupoMaximo = d.CupoMaximo,
                IdCarrera = d.IdCarrera,
                IdSemestre = d.IdSemestre,
                Activo = true
            };
            _context.Seccions.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<object> PutSeccionAsync(int id, SeccionDto d)
        {
            var e = await _context.Seccions.FindAsync(id);
            if (e == null) return null;
            e.NombreSeccion = d.NombreSeccion ?? e.NombreSeccion;
            if (d.CupoMaximo > 0) e.CupoMaximo = d.CupoMaximo;
            if (d.IdCarrera > 0) e.IdCarrera = d.IdCarrera;
            if (d.IdSemestre > 0) e.IdSemestre = d.IdSemestre;
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<bool> DeleteSeccionAsync(int id)
        {
            var e = await _context.Seccions.FindAsync(id);
            if (e == null) return false;
            e.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HabilitarSeccionAsync(int id)
        {
            var e = await _context.Seccions.FindAsync(id);
            if (e == null) return false;
            e.Activo = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetModulosAsync()
        {
            return await _context.ModuloEdificios.ToListAsync();
        }

        public async Task<object> PostModuloAsync(ModuloEdificioDto d)
        {
            var e = new ModuloEdificio { Nombre = d.Nombre, Ubicacion = d.Ubicacion, IdSede = d.IdSede, Activo = true };
            _context.ModuloEdificios.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<object> PutModuloAsync(int id, ModuloEdificioDto d)
        {
            var e = await _context.ModuloEdificios.FindAsync(id);
            if (e == null) return null;
            e.Nombre = d.Nombre ?? e.Nombre;
            e.Ubicacion = d.Ubicacion ?? e.Ubicacion;
            if (d.IdSede > 0) e.IdSede = d.IdSede;
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<bool> DeleteModuloAsync(int id)
        {
            var e = await _context.ModuloEdificios.FindAsync(id);
            if (e == null) return false;
            e.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HabilitarModuloAsync(int id)
        {
            var e = await _context.ModuloEdificios.FindAsync(id);
            if (e == null) return false;
            e.Activo = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetAulasAsync()
        {
            return await _context.Aulas.ToListAsync();
        }

        public async Task<object> PostAulaAsync(AulaDto d)
        {
            var e = new Aula { NumeroSalon = d.NumeroSalon, IdModulo = d.IdModulo, CapacidadAlumnos = d.CapacidadAlumnos, Activo = true };
            _context.Aulas.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<object> PutAulaAsync(int id, AulaDto d)
        {
            var e = await _context.Aulas.FindAsync(id);
            if (e == null) return null;
            e.NumeroSalon = d.NumeroSalon ?? e.NumeroSalon;
            if (d.IdModulo > 0) e.IdModulo = d.IdModulo;
            if (d.CapacidadAlumnos > 0) e.CapacidadAlumnos = d.CapacidadAlumnos;
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<bool> DeleteAulaAsync(int id)
        {
            var e = await _context.Aulas.FindAsync(id);
            if (e == null) return false;
            e.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HabilitarAulaAsync(int id)
        {
            var e = await _context.Aulas.FindAsync(id);
            if (e == null) return false;
            e.Activo = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetPrerrequisitosAsync()
        {
            return await _context.CursoPrerrequisitos.ToListAsync();
        }

        public async Task<object> PostPrerrequisitoAsync(CursoPrerrequisitoDto d)
        {
            var e = new CursoPrerrequisito { IdCurso = d.IdCurso, IdCursoRequerido = d.IdCursoRequerido };
            _context.CursoPrerrequisitos.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<bool> DeletePrerrequisitoAsync(int id)
        {
            var e = await _context.CursoPrerrequisitos.FindAsync(id);
            if (e == null) return false;
            _context.CursoPrerrequisitos.Remove(e);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
