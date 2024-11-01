using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using WEBAPI.data;
//listo
namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriaPrimaController : ControllerBase
    {
        private readonly Conexion _dbConnection;

        public MateriaPrimaController()
        {
            _dbConnection = new Conexion();
        }
        [HttpGet]
        public IActionResult GetMateriasPrimas()
        {
            List<object> materiasPrimas = new List<object>();
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();
                string query = "SELECT * FROM MateriaPrima";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    materiasPrimas.Add(new
                    {
                        ID = reader["ID"],
                        ControlUnitario = reader["ControlUnitario"]
                    });
                }

                return Ok(materiasPrimas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener materias primas: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetMateriaPrimaById(int id)
        {
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();
                string query = "SELECT * FROM MateriaPrima WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var materiaPrima = new
                    {
                        ID = reader["ID"],
                        ControlUnitario = reader["ControlUnitario"]
                    };

                    return Ok(materiaPrima);
                }
                else
                {
                    return NotFound($"Materia Prima con ID {id} no encontrada.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener materia prima: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        public class MateriaPrimaModel
        {
            public string ControlUnitario { get; set; }
        }

        [HttpPost]
        public IActionResult InsertMateriaPrima([FromBody] MateriaPrimaModel nuevaMateriaPrima)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "INSERT INTO MateriaPrima (ControlUnitario) VALUES (@ControlUnitario)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ControlUnitario", nuevaMateriaPrima.ControlUnitario);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Materia Prima insertada correctamente.");
                }
                else
                {
                    return BadRequest("No se pudo insertar la Materia Prima.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar Materia Prima: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
      
        [HttpPut("{id}")]
        public IActionResult UpdateMateriaPrima(int id, [FromBody] MateriaPrimaModel materiaPrimaActualizada)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "UPDATE MateriaPrima SET ControlUnitario = @ControlUnitario WHERE ID = @ID";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@ControlUnitario", materiaPrimaActualizada.ControlUnitario);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Materia Prima actualizada correctamente.");
                }
                else
                {
                    return NotFound($"Materia Prima con ID {id} no encontrada.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar Materia Prima: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
    }
}
