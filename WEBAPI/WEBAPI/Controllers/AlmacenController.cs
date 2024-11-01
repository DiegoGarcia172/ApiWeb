using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using WEBAPI.data;
//listo
namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlmacenController : ControllerBase
    {
        private readonly Conexion _dbConnection;

        public AlmacenController()
        {
            _dbConnection = new Conexion();
        }
        public class AlmacenModel
        {
            public string Nombre { get; set; }
            public int IDPRODUCTO { get; set; }
            public int IDORDEN { get; set; }
        }

        [HttpGet]
        public IActionResult GetAlmacenes()
        {
            List<object> almacenes = new List<object>();
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();
                string query = "SELECT * FROM Almacen";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    almacenes.Add(new
                    {
                        ID = reader["ID"],
                        Nombre = reader["Nombre"],
                        IDPRODUCTO = reader["id_producto"],
                        IDORDEN = reader["id_orden"],
                    }) ;
                }

                return Ok(almacenes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener almacenes: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetAlmacenById(int id)
        {
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();
                string query = "SELECT * FROM Almacen WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var almacen = new
                    {
                        ID = reader["ID"],
                        Nombre = reader["Nombre"],
                        IDPRODUCTO = reader["id_producto"],
                        IDORDEN = reader["id_orden"],
                    };

                    return Ok(almacen);
                }
                else
                {
                    return NotFound($"Almacen con ID {id} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener almacen: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        [HttpPost]
        public IActionResult InsertAlmacen([FromBody] AlmacenModel nuevoAlmacen)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "INSERT INTO Almacen (Nombre, id_producto, id_orden) VALUES (@Nombre, @IDPRODUCTO, @IDORDEN)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", nuevoAlmacen.Nombre);
                cmd.Parameters.AddWithValue("@IDPRODUCTO", nuevoAlmacen.IDPRODUCTO);
                cmd.Parameters.AddWithValue("@IDORDEN", nuevoAlmacen.IDORDEN);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Almacén insertado correctamente.");
                }
                else
                {
                    return BadRequest("No se pudo insertar el almacén.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar almacén: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateAlmacen(int id, [FromBody] AlmacenModel almacenActualizado)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "UPDATE Almacen SET Nombre = @Nombre, id_producto = @IDPRODUCTO, id_orden = @IDORDEN WHERE ID = @ID";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Nombre", almacenActualizado.Nombre);
                cmd.Parameters.AddWithValue("@IDPRODUCTO", almacenActualizado.IDPRODUCTO);
                cmd.Parameters.AddWithValue("@IDORDEN", almacenActualizado.IDORDEN);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Almacén actualizado correctamente.");
                }
                else
                {
                    return NotFound($"Almacén con ID {id} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar almacén: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }

    }
}
