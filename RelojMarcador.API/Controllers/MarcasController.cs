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
        public async Task<IActionResult> ValidarFuncionario([FromBody] Funcionario funcionario)
        {
            var valido = await _service.ValidarFuncionario(funcionario.Identificacion, funcionario.Contrasena);

            if (!valido)
                return Unauthorized(new { success = false, message = "Credenciales inválidas" });

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
            var areas = await _service.ObtenerAreasPorIdentificacion(identificacion);
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

