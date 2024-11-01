using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using WEBAPI.data;
//listo
namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaquinaController : ControllerBase
    {
        private readonly Conexion _dbConnection;

        public MaquinaController()
        {
            _dbConnection = new Conexion();
        }
        [HttpGet]
        public IActionResult GetMaquinas()
        {
            List<object> maquinas = new List<object>();
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();
                string query = "SELECT * FROM Maquina";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    maquinas.Add(new
                    {
                        ID = reader["ID"],
                        Nombre = reader["Nombre"],
                        Estado = reader["Estado"],
                        Tipo = reader["Tipo"],
                        Modelo = reader["Modelo"],
                        id_proceso = reader["id_proceso"],
                        IDOrdenProduccion = reader["id_ordenproduccion"]
                    });
                }

                return Ok(maquinas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener maquinas: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetMaquinaById(int id)
        {
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();
                string query = "SELECT * FROM Maquina WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var maquina = new
                    {
                        ID = reader["ID"],
                        Nombre = reader["Nombre"],
                        Estado = reader["Estado"],
                        Tipo = reader["Tipo"],
                        Modelo = reader["Modelo"],
                        id_proceso = reader["id_proceso"],
                        IDOrdenProduccion = reader["id_ordenproduccion"]
                    };

                    return Ok(maquina);
                }
                else
                {
                    return NotFound($"Maquina con ID {id} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener maquina: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        public class MaquinaModel
        {
            public string Nombre { get; set; }
            public string Estado { get; set; }
            public string Tipo { get; set; }
            public string Modelo { get; set; }
            public int id_proceso { get; set; }
            public int id_ordenproduccion { get; set; }
        }
        [HttpPost]
        public IActionResult InsertMaquina([FromBody] MaquinaModel nuevaMaquina)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "INSERT INTO Maquina (Nombre, Estado, Tipo, Modelo, id_proceso, id_ordenproduccion) " +
                               "VALUES (@Nombre, @Estado, @Tipo, @Modelo, @id_proceso, @id_ordenproduccion)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", nuevaMaquina.Nombre);
                cmd.Parameters.AddWithValue("@Estado", nuevaMaquina.Estado);
                cmd.Parameters.AddWithValue("@Tipo", nuevaMaquina.Tipo);
                cmd.Parameters.AddWithValue("@Modelo", nuevaMaquina.Modelo);
                cmd.Parameters.AddWithValue("@id_proceso", nuevaMaquina.id_proceso);
                cmd.Parameters.AddWithValue("@id_ordenproduccion", nuevaMaquina.id_ordenproduccion);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Maquina insertada correctamente.");
                }
                else
                {
                    return BadRequest("No se pudo insertar la Maquina.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar maquina: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateMaquina(int id, [FromBody] MaquinaModel maquinaActualizada)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "UPDATE Maquina SET Nombre = @Nombre, Estado = @Estado, Tipo = @Tipo, Modelo = @Modelo, " +
                               "id_proceso = @id_proceso, id_ordenproduccion = @id_ordenproduccion WHERE ID = @ID";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Nombre", maquinaActualizada.Nombre);
                cmd.Parameters.AddWithValue("@Estado", maquinaActualizada.Estado);
                cmd.Parameters.AddWithValue("@Tipo", maquinaActualizada.Tipo);
                cmd.Parameters.AddWithValue("@Modelo", maquinaActualizada.Modelo);
                cmd.Parameters.AddWithValue("@id_proceso", maquinaActualizada.id_proceso);
                cmd.Parameters.AddWithValue("@id_ordenproduccion", maquinaActualizada.id_ordenproduccion);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Maquina actualizada correctamente.");
                }
                else
                {
                    return NotFound($"Maquina con ID {id} no encontrada.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar maquina: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }

    }
}
