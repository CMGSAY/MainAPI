using MainAPI.Models.DTOs;
using MainAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarrerasController : ControllerBase
    {
        private readonly ICarrerasService _carrerasService;

        public CarrerasController(ICarrerasService carrerasService)
        {
            _carrerasService = carrerasService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _carrerasService.GetCarrerasAsync());
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Post(CarreraDto d)
        {
            return Ok(await _carrerasService.PostCarreraAsync(d));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Put(int id, CarreraDto d)
        {
            var res = await _carrerasService.PutCarreraAsync(id, d);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _carrerasService.DeleteCarreraAsync(id);
            return res ? Ok(new { mensaje = "Deshabilitado" }) : NotFound();
        }
        
        [HttpPut("{id}/habilitar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Habilitar(int id)
        {
            var res = await _carrerasService.HabilitarCarreraAsync(id);
            return res ? Ok(new { mensaje = "Habilitado" }) : NotFound();
        }

        [HttpPost("{idCarrera}/semestres/{idSemestre}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AsignarSemestre(int idCarrera, int idSemestre)
        {
            var result = await _carrerasService.AsignarSemestreAsync(idCarrera, idSemestre);
            if (!result.IsSuccess) return BadRequest(new { Mensaje = result.Message });

            return Ok(result.Result);
        }

        [HttpPost("carrera-semestre/{idCarreraSemestre}/cursos/{idCurso}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AsignarCurso(int idCarreraSemestre, int idCurso)
        {
            var result = await _carrerasService.AsignarCursoAsync(idCarreraSemestre, idCurso);
            if (!result.IsSuccess) return BadRequest(new { Mensaje = result.Message });

            return Ok(result.Result);
        }

        [HttpPost("vincular-curso-pensum")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> VincularCursoAPensum([FromBody] VincularPensumDto d)
        {
            var result = await _carrerasService.VincularCursoAPensumAsync(d);
            if (!result.IsSuccess) return BadRequest(new { Mensaje = result.Message });

            return Ok(new { Mensaje = result.Message });
        }

        [HttpGet("{idCarrera}/semestres/{idSemestre}/cursos")]
        public async Task<IActionResult> GetCursosPorCarreraYSemestre(int idCarrera, int idSemestre)
        {
            return Ok(await _carrerasService.GetCursosPorCarreraYSemestreAsync(idCarrera, idSemestre));
        }
    }
}