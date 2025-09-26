using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RelojMarcador_Marcas.BusinessLogic;

namespace Frontend_ModuloUsuarios.Pages
{
    public class MarcasModel : PageModel
    {
        private readonly MarcaService _service;

        public MarcasModel(MarcaService service)
        {
            _service = service;
            Areas = new List<SelectListItem>();
        }

        [BindProperty]
        public string Identificacion { get; set; } = "";

        [BindProperty]
        public string Contrasena { get; set; } = "";

        [BindProperty]
        public int IdAreaSeleccionada { get; set; }

        [BindProperty]
        public string TipoMarca { get; set; } = "Entrada";

        [BindProperty]
        public string Detalle { get; set; } = "";

        public List<SelectListItem> Areas { get; set; }
        public string Mensaje { get; set; } = "";

        // Handler AJAX para validar credenciales y devolver áreas
        public async Task<JsonResult> OnPostValidar(string identificacion, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(identificacion) || string.IsNullOrWhiteSpace(contrasena))
                return new JsonResult(new { success = false, message = "Faltan credenciales" });

            var valido = await _service.ValidarFuncionario(identificacion, contrasena);
            if (!valido)
                return new JsonResult(new { success = false, message = "Credenciales inválidas" });

            var areas = await _service.ObtenerAreasPorIdentificacion(identificacion);

            return new JsonResult(new
            {
                success = true,
                areas = areas.Select(a => new { id = a.Id_Area, nombre = a.Nombre_Area })
            });
        }

        // Handler para registrar la marca
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Console.WriteLine($"DEBUG: Identificacion={Identificacion}, Contrasena={Contrasena}, Area={IdAreaSeleccionada}, Tipo={TipoMarca}, Detalle={Detalle}");

                if (string.IsNullOrWhiteSpace(Identificacion) || string.IsNullOrWhiteSpace(Contrasena))
                {
                    Mensaje = "Debe ingresar usuario y contraseña.";
                    return Page();
                }

                var valido = await _service.ValidarFuncionario(Identificacion, Contrasena);
                if (!valido)
                {
                    Mensaje = "Credenciales inválidas";
                    return Page();
                }

                if (IdAreaSeleccionada <= 0)
                {
                    Mensaje = "Debe seleccionar un área.";
                    return Page();
                }

                var idFunc = await _service.ObtenerIDFuncionario(Identificacion);
                var idMarca = await _service.RegistrarMarca(idFunc, IdAreaSeleccionada, Detalle, TipoMarca);

                var horaServidor = DateTime.Now.ToString("HH:mm:ss");

                Mensaje = $"Marca registrada con éxito a las {horaServidor}.";
            }
            catch (Exception ex)
            {
                Mensaje = "Error: " + ex.Message;
            }

            return Page();
        }
    }
}


