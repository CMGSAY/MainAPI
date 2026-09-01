using MainAPI.Models;
using MainAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class CatalogosController : ControllerBase
    {
        private readonly ICatalogosService _catalogosService;

        public CatalogosController(ICatalogosService catalogosService)
        {
            _catalogosService = catalogosService;
        }

        // --- DEPARTAMENTOS ---
        [HttpGet("departamentos")]
        public async Task<IActionResult> GetDepartamentos() => Ok(await _catalogosService.GetDepartamentosAsync());

        [HttpPost("departamentos")]
        public async Task<IActionResult> PostDepartamento([FromBody] Departamento d) => Ok(await _catalogosService.PostDepartamentoAsync(d));

        [HttpPut("departamentos/{id}")]
        public async Task<IActionResult> PutDepartamento(int id, [FromBody] Departamento d)
        {
            var res = await _catalogosService.PutDepartamentoAsync(id, d);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpDelete("departamentos/{id}")]
        public async Task<IActionResult> DeleteDepartamento(int id)
        {
            var res = await _catalogosService.DeleteDepartamentoAsync(id);
            return res ? Ok(new { mensaje = "Deshabilitado" }) : NotFound();
        }
        
        [HttpPut("departamentos/{id}/habilitar")]
        public async Task<IActionResult> HabilitarDepartamento(int id)
        {
            var res = await _catalogosService.HabilitarDepartamentoAsync(id);
            return res ? Ok(new { mensaje = "Habilitado" }) : NotFound();
        }

        // --- MUNICIPIOS ---
        [HttpGet("municipios")]
        public async Task<IActionResult> GetMunicipios() => Ok(await _catalogosService.GetMunicipiosAsync());

        [HttpPost("municipios")]
        public async Task<IActionResult> PostMunicipio([FromBody] Municipio d) => Ok(await _catalogosService.PostMunicipioAsync(d));

        [HttpPut("municipios/{id}")]
        public async Task<IActionResult> PutMunicipio(int id, [FromBody] Municipio d)
        {
            var res = await _catalogosService.PutMunicipioAsync(id, d);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpDelete("municipios/{id}")]
        public async Task<IActionResult> DeleteMunicipio(int id)
        {
            var res = await _catalogosService.DeleteMunicipioAsync(id);
            return res ? Ok(new { mensaje = "Deshabilitado" }) : NotFound();
        }

        [HttpPut("municipios/{id}/habilitar")]
        public async Task<IActionResult> HabilitarMunicipio(int id)
        {
            var res = await _catalogosService.HabilitarMunicipioAsync(id);
            return res ? Ok(new { mensaje = "Habilitado" }) : NotFound();
        }

        // --- SEDES ---
        [HttpGet("sedes")]
        public async Task<IActionResult> GetSedes() => Ok(await _catalogosService.GetSedesAsync());

        [HttpPost("sedes")]
        public async Task<IActionResult> PostSede([FromBody] Sede d) => Ok(await _catalogosService.PostSedeAsync(d));

        [HttpPut("sedes/{id}")]
        public async Task<IActionResult> PutSede(int id, [FromBody] Sede d)
        {
            var res = await _catalogosService.PutSedeAsync(id, d);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpDelete("sedes/{id}")]
        public async Task<IActionResult> DeleteSede(int id)
        {
            var res = await _catalogosService.DeleteSedeAsync(id);
            return res ? Ok(new { mensaje = "Deshabilitado" }) : NotFound();
        }

        [HttpPut("sedes/{id}/habilitar")]
        public async Task<IActionResult> HabilitarSede(int id)
        {
            var res = await _catalogosService.HabilitarSedeAsync(id);
            return res ? Ok(new { mensaje = "Habilitado" }) : NotFound();
        }

        // --- FACULTADES ---
        [HttpGet("facultades")]
        public async Task<IActionResult> GetFacultades() => Ok(await _catalogosService.GetFacultadesAsync());

        [HttpPost("facultades")]
        public async Task<IActionResult> PostFacultad([FromBody] Facultad d) => Ok(await _catalogosService.PostFacultadAsync(d));

        [HttpPut("facultades/{id}")]
        public async Task<IActionResult> PutFacultad(int id, [FromBody] Facultad d)
        {
            var res = await _catalogosService.PutFacultadAsync(id, d);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpDelete("facultades/{id}")]
        public async Task<IActionResult> DeleteFacultad(int id)
        {
            var res = await _catalogosService.DeleteFacultadAsync(id);
            return res ? Ok(new { mensaje = "Deshabilitado" }) : NotFound();
        }

        [HttpPut("facultades/{id}/habilitar")]
        public async Task<IActionResult> HabilitarFacultad(int id)
        {
            var res = await _catalogosService.HabilitarFacultadAsync(id);
            return res ? Ok(new { mensaje = "Habilitado" }) : NotFound();
        }
        
        // --- SEMESTRES ---
        [HttpGet("semestres")]
        public async Task<IActionResult> GetSemestres() => Ok(await _catalogosService.GetSemestresAsync());

        [HttpPost("semestres")]
        public async Task<IActionResult> PostSemestre([FromBody] Semestre d) => Ok(await _catalogosService.PostSemestreAsync(d));

        [HttpPut("semestres/{id}")]
        public async Task<IActionResult> PutSemestre(int id, [FromBody] Semestre d)
        {
            var res = await _catalogosService.PutSemestreAsync(id, d);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpDelete("semestres/{id}")]
        public async Task<IActionResult> DeleteSemestre(int id)
        {
            var res = await _catalogosService.DeleteSemestreAsync(id);
            return res ? Ok(new { mensaje = "Deshabilitado" }) : NotFound();
        }

        [HttpPut("semestres/{id}/habilitar")]
        public async Task<IActionResult> HabilitarSemestre(int id)
        {
            var res = await _catalogosService.HabilitarSemestreAsync(id);
            return res ? Ok(new { mensaje = "Habilitado" }) : NotFound();
        }
    }
}