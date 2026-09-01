using MainAPI.Data;
using MainAPI.Models;
using MainAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public class CarrerasService : ICarrerasService
    {
        private readonly MainDbContext _context;

        public CarrerasService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetCarrerasAsync()
        {
            return await _context.Carreras.ToListAsync();
        }

        public async Task<object> PostCarreraAsync(CarreraDto d)
        {
            var c = new Carrera { IdFacultad = d.IdFacultad, NombreCarrera = d.NombreCarrera, Descripcion = d.Descripcion, CantidadSemestres = d.CantidadSemestres, CreditosTotales = d.CreditosTotales, Activa = true };
            _context.Carreras.Add(c);
            await _context.SaveChangesAsync();
            return c;
        }

        public async Task<object> PutCarreraAsync(int id, CarreraDto d)
        {
            var e = await _context.Carreras.FindAsync(id);
            if (e == null) return null;
            e.NombreCarrera = d.NombreCarrera;
            e.IdFacultad = d.IdFacultad;
            // Opcional: actualizar descripcion, semestres, etc.
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<bool> DeleteCarreraAsync(int id)
        {
            var e = await _context.Carreras.FindAsync(id);
            if (e == null) return false;
            e.Activa = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HabilitarCarreraAsync(int id)
        {
            var e = await _context.Carreras.FindAsync(id);
            if (e == null) return false;
            e.Activa = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool IsSuccess, string Message, object? Result)> AsignarSemestreAsync(int idCarrera, int idSemestre)
        {
            if (await _context.CarreraSemestres.AnyAsync(cs => cs.IdCarrera == idCarrera && cs.IdSemestre == idSemestre))
                return (false, "Ya existe esta asignación.", null);
                
            var e = new CarreraSemestre { IdCarrera = idCarrera, IdSemestre = idSemestre };
            _context.CarreraSemestres.Add(e);
            await _context.SaveChangesAsync();
            return (true, "Asignado exitosamente.", e);
        }

        public async Task<(bool IsSuccess, string Message, object? Result)> AsignarCursoAsync(int idCarreraSemestre, int idCurso)
        {
            if (await _context.CarreraSemestreCursos.AnyAsync(csc => csc.IdCarreraSemestre == idCarreraSemestre && csc.IdCurso == idCurso))
                return (false, "Ya existe esta asignación.", null);
                
            var e = new CarreraSemestreCurso { IdCarreraSemestre = idCarreraSemestre, IdCurso = idCurso };
            _context.CarreraSemestreCursos.Add(e);
            await _context.SaveChangesAsync();
            return (true, "Asignado exitosamente.", e);
        }

        public async Task<(bool IsSuccess, string Message)> VincularCursoAPensumAsync(VincularPensumDto d)
        {
            var carreraSemestre = await _context.CarreraSemestres
                .FirstOrDefaultAsync(cs => cs.IdCarrera == d.IdCarrera && cs.IdSemestre == d.IdSemestre);

            if (carreraSemestre == null)
            {
                carreraSemestre = new CarreraSemestre { IdCarrera = d.IdCarrera, IdSemestre = d.IdSemestre };
                _context.CarreraSemestres.Add(carreraSemestre);
                await _context.SaveChangesAsync(); 
            }

            if (await _context.CarreraSemestreCursos.AnyAsync(csc => csc.IdCarreraSemestre == carreraSemestre.IdCarreraSemestre && csc.IdCurso == d.IdCurso))
            {
                return (false, "Este curso ya está asignado a esta carrera en este semestre.");
            }

            var asignacion = new CarreraSemestreCurso { IdCarreraSemestre = carreraSemestre.IdCarreraSemestre, IdCurso = d.IdCurso };
            _context.CarreraSemestreCursos.Add(asignacion);
            await _context.SaveChangesAsync();

            return (true, "Curso asignado exitosamente al pensum.");
        }

        public async Task<object> GetCursosPorCarreraYSemestreAsync(int idCarrera, int idSemestre)
        {
            var cursos = await (from csc in _context.CarreraSemestreCursos
                                join cs in _context.CarreraSemestres on csc.IdCarreraSemestre equals cs.IdCarreraSemestre
                                join c in _context.Cursos on csc.IdCurso equals c.IdCurso
                                where cs.IdCarrera == idCarrera && cs.IdSemestre == idSemestre
                                select new
                                {
                                    IdCarreraSemestreCurso = csc.IdCarreraSemestreCurso,
                                    NombreCurso = c.NombreCurso
                                }).ToListAsync();
            return cursos;
        }
    }
}
