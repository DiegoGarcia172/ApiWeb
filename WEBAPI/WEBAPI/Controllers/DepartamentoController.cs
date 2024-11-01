using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using WEBAPI.data;
//listo
namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        private readonly Conexion _dbConnection;

        public DepartamentoController()
        {
            _dbConnection = new Conexion();
        }
        public class DepartamentoModel
        {
            public string Nombre { get; set; }
        }

        [HttpGet]
        public IActionResult GetDepartamentos()
        {
            List<object> departamentos = new List<object>();
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();
                string query = "SELECT * FROM Departamento";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    departamentos.Add(new
                    {
                        ID = reader["ID"],
                        Nombre = reader["Nombre"]
                    });
                }

                return Ok(departamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener departamentos: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetDepartamentoById(int id)
        {
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();
                string query = "SELECT * FROM Departamento WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var departamento = new
                    {
                        ID = reader["ID"],
                        Nombre = reader["Nombre"]
                    };

                    return Ok(departamento);
                }
                else
                {
                    return NotFound($"Departamento con ID {id} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener departamento: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        [HttpPost]
        public IActionResult InsertDepartamento([FromBody] DepartamentoModel nuevoDepartamento)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "INSERT INTO Departamento (Nombre) VALUES (@Nombre)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", nuevoDepartamento.Nombre);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Departamento insertado correctamente.");
                }
                else
                {
                    return BadRequest("No se pudo insertar el departamento.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar departamento: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateDepartamento(int id, [FromBody] DepartamentoModel departamentoActualizado)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "UPDATE Departamento SET Nombre = @Nombre WHERE ID = @ID";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Nombre", departamentoActualizado.Nombre);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Departamento actualizado correctamente.");
                }
                else
                {
                    return NotFound($"Departamento con ID {id} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar departamento: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }

    }
}
