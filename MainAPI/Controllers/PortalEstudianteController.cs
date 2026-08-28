using MainAPI.Data;
using MainAPI.Models;
using MainAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Estudiante,Administrador")]
    public class PortalEstudianteController : ControllerBase
    {
        private readonly MainDbContext _context;
        public PortalEstudianteController(MainDbContext context) => _context = context;

        [HttpGet("mis-cursos")]
        public async Task<IActionResult> GetMisCursos() => Ok(await _context.AsignacionCursos.ToListAsync());

        [HttpGet("mis-materiales")]
        public async Task<IActionResult> GetMisMateriales() => Ok(await _context.MaterialClases.ToListAsync());

        [HttpGet("mis-tareas")]
        public async Task<IActionResult> GetMisTareas() => Ok(await _context.Tareas.ToListAsync());

        [HttpPost("entregas")]
        public async Task<IActionResult> PostEntrega(EntregaTareaDto d)
        {
            var e = new EntregaTarea { IdTarea = d.IdTarea, IdEstudiante = d.IdEstudiante, UrlArchivoAdjunto = d.UrlArchivoAdjunto, FechaEnvio = DateTime.Now };
            _context.EntregaTareas.Add(e);
            await _context.SaveChangesAsync();
            return Ok(e);
        }

        [HttpGet("mis-notas")]
        public async Task<IActionResult> GetMisNotas() => Ok(await _context.CalificacionEvaluacions.ToListAsync());

        [HttpGet("kardex")]
        public async Task<IActionResult> GetKardex()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized("Token inválido.");
            int userId = int.Parse(userIdString);

            var estudiante = await _context.PerfilEstudiantes.FirstOrDefaultAsync(p => p.IdPersonaNavigation.LoginUserId == userId);
            if (estudiante == null) return Unauthorized("Perfil no encontrado.");

            var historial = await _context.AsignacionCursos
                .Include(a => a.IdCursoHabilitadoNavigation.IdCarreraSemestreCursoNavigation.IdCursoNavigation)
                .Where(a => a.IdEstudiante == estudiante.IdEstudiante)
                .Select(a => new {
                    Curso = a.IdCursoHabilitadoNavigation.IdCarreraSemestreCursoNavigation.IdCursoNavigation.NombreCurso,
                    NotaFinal = a.NotaFinal,
                    Estado = a.NotaFinal >= 61 ? "Aprobado" : "Reprobado"
                }).ToListAsync();

            return Ok(historial);
        }
        // 4. CURSOS DISPONIBLES PARA AUTO-MATRICULACIÓN
        // GET: api/PortalEstudiante/5/cursos-disponibles-matricula
        [HttpGet("{idEstudiante}/cursos-disponibles-matricula")]
        public async Task<IActionResult> GetCursosDisponiblesMatricula(int idEstudiante)
        {
            var estudiante = await _context.PerfilEstudiantes.FindAsync(idEstudiante);
            if (estudiante == null || estudiante.IdSemestreActual == null || estudiante.IdCarrera == null)
                return BadRequest("El estudiante no tiene un semestre o carrera oficializada por el Administrador.");

            var infoAcademica = await (from c in _context.Carreras
                                       join f in _context.Facultads on c.IdFacultad equals f.IdFacultad
                                       join s in _context.Sedes on f.IdSede equals s.IdSede
                                       join m in _context.Municipios on s.IdMunicipio equals m.IdMunicipio
                                       join d in _context.Departamentos on m.IdDepartamento equals d.IdDepartamento
                                       where c.IdCarrera == estudiante.IdCarrera
                                       select new
                                       {
                                           Facultad = f.NombreFacultad,
                                           Sede = s.Nombre,
                                           Departamento = d.NombreDepartamento
                                       }).FirstOrDefaultAsync();

            var cursosDisponibles = await (from ch in _context.CursoHabilitados
                                           join csc in _context.CarreraSemestreCursos on ch.IdCarreraSemestreCurso equals csc.IdCarreraSemestreCurso
                                           join cs in _context.CarreraSemestres on csc.IdCarreraSemestre equals cs.IdCarreraSemestre
                                           join cur in _context.Cursos on csc.IdCurso equals cur.IdCurso
                                           join sec in _context.Seccions on ch.IdSeccion equals sec.IdSeccion
                                           join pc in _context.PerfilCatedraticos on ch.IdCatedratico equals pc.IdCatedratico
                                           join per in _context.Personas on pc.IdPersona equals per.IdPersona
                                           where cs.IdSemestre == estudiante.IdSemestreActual && cs.IdCarrera == estudiante.IdCarrera && ch.Estado == "activo"
                                          select new
                                          {
                                               IdCursoHabilitado = ch.IdCursoHabilitado,
                                               NombreCurso = cur.NombreCurso,
                                               Seccion = sec.NombreSeccion,
                                               Catedratico = $"{per.PrimerNombre} {per.PrimerApellido}",
                                               Horario = $"{ch.HorarioInicio}-{ch.HorarioFin}"
                                           }).ToListAsync();

            var respuesta = new
            {
                Facultad = infoAcademica?.Facultad ?? "N/A",
                Departamento = infoAcademica?.Departamento ?? "N/A",
                Sede = infoAcademica?.Sede ?? "N/A",
                Cursos = cursosDisponibles
            };

            return Ok(respuesta);
        }

    }
}