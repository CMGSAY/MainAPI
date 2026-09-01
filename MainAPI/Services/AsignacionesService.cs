using MainAPI.Data;
using MainAPI.Models;
using MainAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public class AsignacionesService : IAsignacionesService
    {
        private readonly MainDbContext _context;

        public AsignacionesService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetCursosHabilitadosAsync()
        {
            return await _context.CursoHabilitados.ToListAsync();
        }

        public async Task<object> GetHorariosOcupadosAsync(int idSeccion, int idAula, string dia)
        {
            var horariosDb = await _context.HorarioCursos
                .Include(h => h.IdCursoHabilitadoNavigation)
                .Where(h => h.DiaSemana == dia &&
                           (h.IdCursoHabilitadoNavigation.IdSeccion == idSeccion || h.IdCursoHabilitadoNavigation.IdAula == idAula) &&
                           h.IdCursoHabilitadoNavigation.Estado == "activo")
                .Select(h => new { h.HoraInicio, h.HoraFin, h.IdCursoHabilitadoNavigation.IdSeccion, h.IdCursoHabilitadoNavigation.IdAula })
                .ToListAsync();

            return horariosDb.Select(h => new {
                HoraInicio = h.HoraInicio.ToString("HH:mm:ss"),
                HoraFin = h.HoraFin.ToString("HH:mm:ss"),
                h.IdSeccion,
                h.IdAula
            }).ToList();
        }

        public async Task<(bool IsSuccess, string Message)> PostCursoHabilitadoAsync(CursoHabilitadoDto d)
        {
            try
            {
                if (d.Horarios != null && d.Horarios.Any())
                {
                    foreach (var h in d.Horarios)
                    {
                        var reqInicio = TimeOnly.Parse(h.HoraInicio);
                        var reqFin = TimeOnly.Parse(h.HoraFin);

                        var choqueSeccion = await _context.HorarioCursos
                            .Include(hc => hc.IdCursoHabilitadoNavigation)
                            .AnyAsync(hc => hc.IdCursoHabilitadoNavigation.IdSeccion == d.IdSeccion &&
                                            hc.IdCursoHabilitadoNavigation.IdCiclo == d.IdCiclo &&
                                            hc.IdCursoHabilitadoNavigation.Estado == "activo" &&
                                            hc.DiaSemana == h.DiaSemana &&
                                            ((reqInicio >= hc.HoraInicio && reqInicio < hc.HoraFin) ||
                                             (reqFin > hc.HoraInicio && reqFin <= hc.HoraFin) ||
                                             (reqInicio <= hc.HoraInicio && reqFin >= hc.HoraFin)));

                        if (choqueSeccion) return (false, $"Choque de horario detectado: La sección ya tiene clases el {h.DiaSemana} en ese horario.");

                        var choqueAula = await _context.HorarioCursos
                            .Include(hc => hc.IdCursoHabilitadoNavigation)
                            .AnyAsync(hc => hc.IdCursoHabilitadoNavigation.IdAula == d.IdAula &&
                                            hc.IdCursoHabilitadoNavigation.IdCiclo == d.IdCiclo &&
                                            hc.IdCursoHabilitadoNavigation.Estado == "activo" &&
                                            hc.DiaSemana == h.DiaSemana &&
                                            ((reqInicio >= hc.HoraInicio && reqInicio < hc.HoraFin) ||
                                             (reqFin > hc.HoraInicio && reqFin <= hc.HoraFin) ||
                                             (reqInicio <= hc.HoraInicio && reqFin >= hc.HoraFin)));

                        if (choqueAula) return (false, $"Choque de horario detectado: El Aula/Salón ya está ocupada el {h.DiaSemana} en ese horario.");
                    }
                }

                var primerHorario = d.Horarios?.FirstOrDefault();
                var e = new CursoHabilitado
                {
                    IdCarreraSemestreCurso = d.IdCarreraSemestreCurso,
                    IdCiclo = d.IdCiclo,
                    IdJornada = d.IdJornada,
                    IdSeccion = d.IdSeccion,
                    IdAula = d.IdAula,
                    IdCatedratico = d.IdCatedratico,
                    Estado = d.Estado ?? "activo",
                    HorarioInicio = primerHorario != null ? TimeOnly.Parse(primerHorario.HoraInicio) : null,
                    HorarioFin = primerHorario != null ? TimeOnly.Parse(primerHorario.HoraFin) : null
                };

                _context.CursoHabilitados.Add(e);
                await _context.SaveChangesAsync();

                if (d.Horarios != null && d.Horarios.Any())
                {
                    foreach (var h in d.Horarios)
                    {
                        _context.HorarioCursos.Add(new HorarioCurso
                        {
                            IdCursoHabilitado = e.IdCursoHabilitado,
                            DiaSemana = h.DiaSemana,
                            HoraInicio = TimeOnly.Parse(h.HoraInicio),
                            HoraFin = TimeOnly.Parse(h.HoraFin)
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                return (true, "Curso habilitado exitosamente");
            }
            catch (Exception ex)
            {
                var errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return (false, $"Error interno: {errorMsg}");
            }
        }

        public async Task<object> GetAsignacionesAsync()
        {
            return await _context.AsignacionCursos.ToListAsync();
        }

        public async Task<object> PostAsignacionAsync(AsignacionCursoDto d)
        {
            var e = new AsignacionCurso { IdEstudiante = d.IdEstudiante, IdCursoHabilitado = d.IdCursoHabilitado, FechaAsignacion = DateOnly.FromDateTime(DateTime.Now), Estado = "asignado", NotaFinal = 0 };
            _context.AsignacionCursos.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<(bool IsSuccess, string Message)> PostMatriculaMultipleAsync(MatriculaMultipleDto d)
        {
            var conf = await _context.ConfiguracionSistemas.FirstOrDefaultAsync(c => c.Clave == "inscripciones_abiertas");
            if (conf == null || conf.Valor != "true")
            {
                return (false, "El proceso de inscripción/asignación está cerrado actualmente.");
            }

            foreach (var idCurso in d.IdsCursosHabilitados)
            {
                int inscritos = await _context.AsignacionCursos.CountAsync(a => a.IdCursoHabilitado == idCurso);
                if (inscritos >= 40)
                {
                    return (false, $"Cupo máximo alcanzado (40) para el curso ID {idCurso}. Por favor asigne otra sección.");
                }

                var e = new AsignacionCurso { IdEstudiante = d.IdEstudiante, IdCursoHabilitado = idCurso, FechaAsignacion = DateOnly.FromDateTime(DateTime.Now), Estado = "asignado", NotaFinal = 0 };
                _context.AsignacionCursos.Add(e);
            }
            await _context.SaveChangesAsync();
            return (true, "Cursos matriculados exitosamente");
        }

        public async Task<object> GetCursosHabilitadosPorPensumAsync(int idCarreraSemestreCurso)
        {
            var res = await (from ch in _context.CursoHabilitados
                             join sec in _context.Seccions on ch.IdSeccion equals sec.IdSeccion
                             join pc in _context.PerfilCatedraticos on ch.IdCatedratico equals pc.IdCatedratico
                             join per in _context.Personas on pc.IdPersona equals per.IdPersona
                             where ch.IdCarreraSemestreCurso == idCarreraSemestreCurso && ch.Estado == "activo"
                             select new
                             {
                                 IdCursoHabilitado = ch.IdCursoHabilitado,
                                 DescripcionLarga = $"Sección {sec.NombreSeccion} - Docente: {per.PrimerNombre} {per.PrimerApellido} ({ch.HorarioInicio}-{ch.HorarioFin})"
                             }).ToListAsync();
            return res;
        }

        public async Task<object> GetCursosActivosAsync()
        {
            var query = await (from ch in _context.CursoHabilitados
                               join sec in _context.Seccions on ch.IdSeccion equals sec.IdSeccion
                               join csc in _context.CarreraSemestreCursos on ch.IdCarreraSemestreCurso equals csc.IdCarreraSemestreCurso
                               join cs in _context.CarreraSemestres on csc.IdCarreraSemestre equals cs.IdCarreraSemestre
                               join sem in _context.Semestres on cs.IdSemestre equals sem.IdSemestre
                               join car in _context.Carreras on cs.IdCarrera equals car.IdCarrera
                               join fac in _context.Facultads on car.IdFacultad equals fac.IdFacultad
                               join c in _context.Cursos on csc.IdCurso equals c.IdCurso
                               join pc in _context.PerfilCatedraticos on ch.IdCatedratico equals pc.IdCatedratico
                               join per in _context.Personas on pc.IdPersona equals per.IdPersona
                               where ch.Estado == "activo"
                               select new
                               {
                                   ch.IdCursoHabilitado,
                                   c.NombreCurso,
                                   sec.NombreSeccion,
                                   Docente = per.PrimerNombre + " " + per.PrimerApellido,
                                   fac.NombreFacultad,
                                   sem.NombreSemestre,
                                   car.NombreCarrera,
                                   ch.HorarioInicio,
                                   ch.HorarioFin,
                                   Dias = _context.HorarioCursos.Where(h => h.IdCursoHabilitado == ch.IdCursoHabilitado).Select(h => h.DiaSemana).ToList()
                               }).ToListAsync();

            var res = query.Select(q => new
            {
                IdCursoHabilitado = q.IdCursoHabilitado,
                DisplayString = $"{q.NombreCurso} (Sec: {q.NombreSeccion}) - {q.Docente}",
                Detalles = $"Facultad: {q.NombreFacultad}\nCarrera: {q.NombreCarrera} | Semestre: {q.NombreSemestre}\nDías: {string.Join(", ", q.Dias)} | Horario: {q.HorarioInicio}-{q.HorarioFin}"
            });

            return res;
        }

        public async Task<(bool IsSuccess, string Message)> DesactivarCursoAsync(int idCursoHabilitado)
        {
            var curso = await _context.CursoHabilitados.FindAsync(idCursoHabilitado);
            if (curso == null) return (false, "Curso no encontrado.");

            curso.Estado = "inactivo";
            await _context.SaveChangesAsync();
            return (true, "Curso deshabilitado exitosamente. Los horarios han sido liberados.");
        }
    }
}
