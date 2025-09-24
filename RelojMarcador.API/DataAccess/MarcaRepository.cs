using Dapper;
using MySqlConnector;
using RelojMarcador.API.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RelojMarcador.API.DataAccess
{
    public class MarcaRepository
    {
        private readonly string _connectionString;

        public MarcaRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MySql")
                ?? throw new ArgumentNullException("Connection string 'MySql' not found.");
        }

        public async Task<bool> ValidarFuncionario(string identificacion, string contrasena)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM Funcionarios
                WHERE IDENTIFICACION = @identificacion
                  AND CONTRASENA = @contrasena
                  AND ESTADO = 'Activo';";

            await using var connection = new MySqlConnection(_connectionString);
            var result = await connection.ExecuteScalarAsync<int>(sql, new
            {
                identificacion,
                contrasena
            });

            return result > 0; // si hay registros, devuelve true
        }

        public async Task<int> ObtenerIDFuncionario(string identificacion)
        {
            const string sql = @"SELECT ID_Funcionario 
                         FROM Funcionarios 
                         WHERE Identificacion = @Identificacion";

            await using var db = new MySqlConnection(_connectionString);

            // Devuelve null si no encuentra el registro
            var result = await db.ExecuteScalarAsync<int?>(sql, new { Identificacion = identificacion });

            return result ?? 0;
        }

        public async Task<IEnumerable<Area>> ObtenerAreasFuncionario(string identificacion)
        {
            const string sql = @"
                SELECT a.ID_Area, a.Nombre_Area
                FROM Funcionarios f
                INNER JOIN Funcionario_Area fa ON f.ID_Funcionario = fa.ID_Funcionario
                INNER JOIN Areas a ON fa.ID_Area = a.ID_Area
                WHERE f.Identificacion = @Identificacion;";

            await using var db = new MySqlConnection(_connectionString);

            var result = await db.QueryAsync<Area>(sql, new { Identificacion = identificacion });

            return result;
        }

        public async Task<int> RegistrarMarca(int idFuncionario, int idArea, string detalle, string tipoMarca)
        {
            const string sql = @"
                INSERT INTO Marca (ID_Funcionario, ID_Area, Detalle, Tipo_Marca)
                VALUES (@idFuncionario, @idArea, @detalle, @tipoMarca);
                SELECT LAST_INSERT_ID();";

            await using var db = new MySqlConnection(_connectionString);

            var id = await db.ExecuteScalarAsync<int>(sql, new
            {
                idFuncionario,
                idArea,
                detalle,
                tipoMarca
            });

            return id;
        }

    }
}
