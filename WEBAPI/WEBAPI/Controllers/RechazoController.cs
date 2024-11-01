using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using WEBAPI.data;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RechazoController : ControllerBase
    {
        private readonly Conexion _dbConnection;

        public RechazoController()
        {
            _dbConnection = new Conexion();
        }
        [HttpGet]
        public IActionResult GetRechazos()
        {
            List<object> rechazos = new List<object>();
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();
                string query = "SELECT * FROM Rechazo";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    rechazos.Add(new
                    {
                        ID = reader["ID"],
                        CantidadPR = reader["CantidadPR"],
                        Descripcion = reader["Descripcion"],
                        Fecha_Hora = reader["Fecha_Hora"],
                        IDProducto = reader["id_producto"]
                    });
                }

                return Ok(rechazos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener rechazos: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetRechazoById(int id)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "SELECT * FROM Rechazo WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var rechazo = new
                    {
                        ID = reader["ID"],
                        CantidadPR = reader["CantidadPR"],
                        Descripcion = reader["Descripcion"],
                        Fecha_Hora = reader["Fecha_Hora"],
                        IDProducto = reader["id_producto"]
                    };

                    return Ok(rechazo);
                }
                else
                {
                    return NotFound($"Rechazo con ID {id} no encontrado."); 
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener rechazo: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }


        public class RechazoModel
        {
            public int CantidadPR { get; set; }
            public string Descripcion { get; set; }
            public DateTime Fecha_Hora { get; set; }
            public int IDProducto { get; set; }

        }

        [HttpPost]
        public IActionResult InsertRechazo([FromBody] RechazoModel nuevoRechazo)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "INSERT INTO Rechazo (CantidadPR, Descripcion, Fecha_Hora, id_producto) " +
                               "VALUES (@CantidadPR, @Descripcion, @Fecha_Hora, @IDProducto)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@CantidadPR", nuevoRechazo.CantidadPR);
                cmd.Parameters.AddWithValue("@Descripcion", nuevoRechazo.Descripcion);
                cmd.Parameters.AddWithValue("@Fecha_Hora", nuevoRechazo.Fecha_Hora);
                cmd.Parameters.AddWithValue("@IDProducto", nuevoRechazo.IDProducto);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Rechazo insertado correctamente.");
                }
                else
                {
                    return BadRequest("No se pudo insertar el rechazo.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar rechazo: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateRechazo(int id, [FromBody] RechazoModel updatedRechazo)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "UPDATE Rechazo SET CantidadPR = @CantidadPR, Descripcion = @Descripcion, " +
                               "Fecha_Hora = @Fecha_Hora, id_producto = @IDProducto WHERE ID = @ID";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@CantidadPR", updatedRechazo.CantidadPR);
                cmd.Parameters.AddWithValue("@Descripcion", updatedRechazo.Descripcion);
                cmd.Parameters.AddWithValue("@Fecha_Hora", updatedRechazo.Fecha_Hora);
                cmd.Parameters.AddWithValue("@IDProducto", updatedRechazo.IDProducto);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Rechazo actualizado correctamente.");
                }
                else
                {
                    return NotFound($"Rechazo con ID {id} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar rechazo: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
    }
}
