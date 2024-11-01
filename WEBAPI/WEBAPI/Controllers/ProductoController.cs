using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using WEBAPI.data;
//listo
namespace TuProyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly Conexion _dbConnection;

        public ProductoController()
        {
            _dbConnection = new Conexion();
        }
        [HttpGet]
        public IActionResult GetProductos()
        {
            List<object> productos = new List<object>();
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();
                string query = "SELECT * FROM Producto";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productos.Add(new
                    {
                        ID = reader["ID"],
                        Nombre = reader["Nombre"],
                        Cantidad = reader["Cantidad"],
                        Calidad = reader["Calidad"],
                        Descripcion = reader["Descripcion"],
                        Fecha_Fin = reader["Fecha_Fin"]
                    });
                }

                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener productos: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetProductoById(int id)
        {
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();
                string query = "SELECT * FROM Producto WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var producto = new
                    {
                        ID = reader["ID"],
                        Cantidad = reader["Cantidad"],
                        Calidad = reader["Calidad"],
                        Descripcion = reader["Descripcion"],
                        Fecha_Fin = reader["Fecha_Fin"]
                    };

                    return Ok(producto);
                }
                else
                {
                    return NotFound($"Producto con ID {id} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener producto: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        public class ProductoModel
        {
            public string Nombre { get; set; }
            public int Cantidad { get; set; }
            public string Calidad { get; set; }
            public string Descripcion { get; set; }
            public DateTime Fecha_Fin { get; set; }
        }

        [HttpPost]
        public IActionResult InsertProducto([FromBody] ProductoModel nuevoProducto)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "INSERT INTO Producto (Nombre, Cantidad, Calidad, Descripcion, Fecha_Fin) " +
                               "VALUES (@Nombre, @Cantidad, @Calidad, @Descripcion, @Fecha_Fin)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", nuevoProducto.Nombre);
                cmd.Parameters.AddWithValue("@Cantidad", nuevoProducto.Cantidad);
                cmd.Parameters.AddWithValue("@Calidad", nuevoProducto.Calidad);
                cmd.Parameters.AddWithValue("@Descripcion", nuevoProducto.Descripcion);
                cmd.Parameters.AddWithValue("@Fecha_Fin", nuevoProducto.Fecha_Fin);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Producto insertado correctamente.");
                }
                else
                {
                    return BadRequest("No se pudo insertar el producto.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar producto: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateProducto(int id, [FromBody] ProductoModel productoActualizado)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "UPDATE Producto SET Nombre = @Nombre, Cantidad = @Cantidad, Calidad = @Calidad, " +
                               "Descripcion = @Descripcion, Fecha_Fin = @Fecha_Fin WHERE ID = @ID";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Nombre", productoActualizado.Nombre);
                cmd.Parameters.AddWithValue("@Cantidad", productoActualizado.Cantidad);
                cmd.Parameters.AddWithValue("@Calidad", productoActualizado.Calidad);
                cmd.Parameters.AddWithValue("@Descripcion", productoActualizado.Descripcion);
                cmd.Parameters.AddWithValue("@Fecha_Fin", productoActualizado.Fecha_Fin);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Producto actualizado correctamente.");
                }
                else
                {
                    return NotFound($"Producto con ID {id} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar producto: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }

    }
}
