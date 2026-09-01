using MainAPI.Models.DTOs;
using MainAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class OperativoController : ControllerBase
    {
        private readonly IOperativoService _operativoService;

        public OperativoController(IOperativoService operativoService)
        {
            _operativoService = operativoService;
        }

        [HttpGet("ciclos")]
        public async Task<IActionResult> GetCiclos()
        {
            return Ok(await _operativoService.GetCiclosAsync());
        }

        [HttpPost("ciclos")]
        public async Task<IActionResult> PostCiclo(CicloEscolarDto d)
        {
            return Ok(await _operativoService.PostCicloAsync(d));
        }

        [HttpPut("ciclos/{id}")]
        public async Task<IActionResult> PutCiclo(int id, CicloEscolarDto d)
        {
            var res = await _operativoService.PutCicloAsync(id, d);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpDelete("ciclos/{id}")]
        public async Task<IActionResult> DeleteCiclo(int id)
        {
            var res = await _operativoService.DeleteCicloAsync(id);
            return res ? Ok(new { mensaje = "Deshabilitado" }) : NotFound();
        }

        [HttpPut("ciclos/{id}/habilitar")]
        public async Task<IActionResult> HabilitarCiclo(int id)
        {
            var res = await _operativoService.HabilitarCicloAsync(id);
            return res ? Ok(new { mensaje = "Habilitado" }) : NotFound();
        }

        [HttpGet("jornadas")]
        public async Task<IActionResult> GetJornadas()
        {
            return Ok(await _operativoService.GetJornadasAsync());
        }

        [HttpPost("jornadas")]
        public async Task<IActionResult> PostJornada(JornadaDto d)
        {
            return Ok(await _operativoService.PostJornadaAsync(d));
        }

        [HttpPut("jornadas/{id}")]
        public async Task<IActionResult> PutJornada(int id, JornadaDto d)
        {
            var res = await _operativoService.PutJornadaAsync(id, d);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpDelete("jornadas/{id}")]
        public async Task<IActionResult> DeleteJornada(int id)
        {
            var res = await _operativoService.DeleteJornadaAsync(id);
            return res ? Ok(new { mensaje = "Deshabilitado" }) : NotFound();
        }

        [HttpPut("jornadas/{id}/habilitar")]
        public async Task<IActionResult> HabilitarJornada(int id)
        {
            var res = await _operativoService.HabilitarJornadaAsync(id);
            return res ? Ok(new { mensaje = "Habilitado" }) : NotFound();
        }

        [HttpGet("secciones")]
        public async Task<IActionResult> GetSecciones()
        {
            return Ok(await _operativoService.GetSeccionesAsync());
        }

        [HttpPost("secciones")]
        public async Task<IActionResult> PostSeccion(SeccionDto d)
        {
            return Ok(await _operativoService.PostSeccionAsync(d));
        }

        [HttpPut("secciones/{id}")]
        public async Task<IActionResult> PutSeccion(int id, SeccionDto d)
        {
            var res = await _operativoService.PutSeccionAsync(id, d);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpDelete("secciones/{id}")]
        public async Task<IActionResult> DeleteSeccion(int id)
        {
            var res = await _operativoService.DeleteSeccionAsync(id);
            return res ? Ok(new { mensaje = "Deshabilitado" }) : NotFound();
        }

        [HttpPut("secciones/{id}/habilitar")]
        public async Task<IActionResult> HabilitarSeccion(int id)
        {
            var res = await _operativoService.HabilitarSeccionAsync(id);
            return res ? Ok(new { mensaje = "Habilitado" }) : NotFound();
        }

        [HttpGet("modulos")]
        public async Task<IActionResult> GetModulos()
        {
            return Ok(await _operativoService.GetModulosAsync());
        }

        [HttpPost("modulos")]
        public async Task<IActionResult> PostModulo(ModuloEdificioDto d)
        {
            return Ok(await _operativoService.PostModuloAsync(d));
        }

        [HttpPut("modulos/{id}")]
        public async Task<IActionResult> PutModulo(int id, ModuloEdificioDto d)
        {
            var res = await _operativoService.PutModuloAsync(id, d);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpDelete("modulos/{id}")]
        public async Task<IActionResult> DeleteModulo(int id)
        {
            var res = await _operativoService.DeleteModuloAsync(id);
            return res ? Ok(new { mensaje = "Deshabilitado" }) : NotFound();
        }

        [HttpPut("modulos/{id}/habilitar")]
        public async Task<IActionResult> HabilitarModulo(int id)
        {
            var res = await _operativoService.HabilitarModuloAsync(id);
            return res ? Ok(new { mensaje = "Habilitado" }) : NotFound();
        }

        [HttpGet("aulas")]
        public async Task<IActionResult> GetAulas()
        {
            return Ok(await _operativoService.GetAulasAsync());
        }

        [HttpPost("aulas")]
        public async Task<IActionResult> PostAula(AulaDto d)
        {
            return Ok(await _operativoService.PostAulaAsync(d));
        }

        [HttpPut("aulas/{id}")]
        public async Task<IActionResult> PutAula(int id, AulaDto d)
        {
            var res = await _operativoService.PutAulaAsync(id, d);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpDelete("aulas/{id}")]
        public async Task<IActionResult> DeleteAula(int id)
        {
            var res = await _operativoService.DeleteAulaAsync(id);
            return res ? Ok(new { mensaje = "Deshabilitado" }) : NotFound();
        }

        [HttpPut("aulas/{id}/habilitar")]
        public async Task<IActionResult> HabilitarAula(int id)
        {
            var res = await _operativoService.HabilitarAulaAsync(id);
            return res ? Ok(new { mensaje = "Habilitado" }) : NotFound();
        }

        [HttpGet("prerrequisitos")]
        public async Task<IActionResult> GetPrerreq()
        {
            return Ok(await _operativoService.GetPrerrequisitosAsync());
        }

        [HttpPost("prerrequisitos")]
        public async Task<IActionResult> PostPrerreq(CursoPrerrequisitoDto d)
        {
            return Ok(await _operativoService.PostPrerrequisitoAsync(d));
        }

        [HttpDelete("prerrequisitos/{id}")]
        public async Task<IActionResult> DeletePrerreq(int id)
        {
            var res = await _operativoService.DeletePrerrequisitoAsync(id);
            return res ? Ok(new { mensaje = "Eliminado" }) : NotFound();
        }
    }
}