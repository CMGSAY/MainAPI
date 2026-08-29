using MainAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HorarioSemanalController : ControllerBase
    {
        private readonly MainDbContext _context;
        public HorarioSemanalController(MainDbContext context) => _context = context;

        [HttpGet("{idSeccion}/jornada/{idJornada}")]
        public async Task<IActionResult> GetHorarioSemanal(int idSeccion, int idJornada)
        {
            var horarios = await (from hc in _context.HorarioCursos
                                  join ch in _context.CursoHabilitados on hc.IdCursoHabilitado equals ch.IdCursoHabilitado
                                  join csc in _context.CarreraSemestreCursos on ch.IdCarreraSemestreCurso equals csc.IdCarreraSemestreCurso
                                  join c in _context.Cursos on csc.IdCurso equals c.IdCurso
                                  join pc in _context.PerfilCatedraticos on ch.IdCatedratico equals pc.IdCatedratico
                                  join per in _context.Personas on pc.IdPersona equals per.IdPersona
                                  join a in _context.Aulas on ch.IdAula equals a.IdAula
                                  where ch.IdSeccion == idSeccion && ch.IdJornada == idJornada && ch.Estado == "activo"
                                  select new
                                  {
                                      DiaSemana = hc.DiaSemana,
                                      HoraInicio = hc.HoraInicio.ToString(@"hh\:mm"),
                                      HoraFin = hc.HoraFin.ToString(@"hh\:mm"),
                                      Curso = c.NombreCurso,
                                      Catedratico = $"{per.PrimerNombre} {per.PrimerApellido}",
                                      Salon = a.NumeroSalon
                                  }).ToListAsync();

            return Ok(horarios);
        }
    }
}