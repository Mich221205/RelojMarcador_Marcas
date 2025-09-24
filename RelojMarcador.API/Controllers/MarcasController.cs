using Microsoft.AspNetCore.Mvc;
using RelojMarcador.API.BusinessLogic;
using RelojMarcador.API.Entities;

namespace RelojMarcador.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarcasController : ControllerBase
    {
        private readonly MarcaService _service;

        public MarcasController(MarcaService service)
        {
            _service = service;
        }

        [HttpPost("validar-funcionario")]
        public async Task<IActionResult> ValidarFuncionario([FromBody] dynamic login)
        {
            string identificacion = login.identificacion;
            string contrasena = login.contrasena;

            var valido = await _service.ValidarFuncionario(identificacion, contrasena);

            if (!valido) return Unauthorized(new { success = false, message = "Credenciales inválidas" });
            return Ok(new { success = true });
        }

        [HttpGet("funcionario/{identificacion}/id")]
        public async Task<IActionResult> ObtenerID(string identificacion)
        {
            var id = await _service.ObtenerIDFuncionario(identificacion);
            return Ok(new { id });
        }

        [HttpGet("funcionario/{identificacion}/areas")]
        public async Task<IActionResult> ObtenerAreas(string identificacion)
        {
            var areas = await _service.ObtenerAreasFuncionario(identificacion);
            return Ok(areas);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarMarca([FromBody] Marca marca)
        {
            var id = await _service.RegistrarMarca(marca.ID_Funcionario, marca.ID_Area, marca.Detalle, marca.Tipo_Marca);
            return Ok(new { success = true, idMarca = id });
        }
    }
}

