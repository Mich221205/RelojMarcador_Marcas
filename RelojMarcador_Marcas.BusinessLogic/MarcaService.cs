using RelojMarcador_Marcas.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RelojMarcador_Marcas.DataAccess;


namespace RelojMarcador_Marcas.BusinessLogic
{
    public class MarcaService
    {
        private readonly MarcaRepository _repo;

        public MarcaService(MarcaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Area>> ObtenerAreasPorIdentificacion(string identificacion)
        {
            var idFuncionario = await _repo.ObtenerIDFuncionario(identificacion);

            if (idFuncionario == 0)
                return Enumerable.Empty<Area>(); // no existe el funcionario

            return await _repo.ObtenerAreasFuncionario(idFuncionario);
        }


        public async Task<bool> ValidarFuncionario(string identificacion, string contrasena)
        {
            return await _repo.ValidarFuncionario(identificacion, contrasena);
        }

        public async Task<int> ObtenerIDFuncionario(string identificacion)
        {
            return await _repo.ObtenerIDFuncionario(identificacion);
        }

        public async Task<int> RegistrarMarca(int idFuncionario, int idArea, string detalle, string tipoMarca)
        {
            return await _repo.RegistrarMarca(idFuncionario, idArea, detalle, tipoMarca);
        }
    }
}
