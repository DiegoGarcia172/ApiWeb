using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using WEBAPI.data;
//listo
namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenProduccionController : ControllerBase
    {
        private readonly Conexion _dbConnection;

        public OrdenProduccionController()
        {
            _dbConnection = new Conexion();
        }
        [HttpGet]
        public IActionResult GetOrdenesProduccion()
        {
            List<object> ordenesProduccion = new List<object>();
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();
                string query = "SELECT * FROM OrdenProduccion";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ordenesProduccion.Add(new
                    {
                        ID = reader["ID"],
                        Cantidad = reader["Cantidad"],
                        FechaOrden = reader["Fecha_Orden"],
                        FechaEntrega = reader["Fecha_Entrega"],
                        ID_Empleado = reader["id_empleado"]
                    });
                }

                return Ok(ordenesProduccion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener ordenes de produccion: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetOrdenProduccionById(int id)
        {
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();
                string query = "SELECT * FROM OrdenProduccion WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var ordenProduccion = new
                    {
                        ID = reader["ID"],
                        Cantidad = reader["Cantidad"],
                        FechaOrden = reader["Fecha_Orden"],
                        FechaEntrega = reader["Fecha_Entrega"],
                        ID_Empleado = reader["id_empleado"]
                    };

                    return Ok(ordenProduccion);
                }
                else
                {
                    return NotFound($"Orden de Produccion con ID {id} no encontrada.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener orden de produccion: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        public class OrdenProduccionModel
        {
            public int Cantidad { get; set; }
            public DateTime FechaOrden { get; set; }
            public DateTime FechaEntrega { get; set; }
            public int ID_Empleado { get; set; }
        }

        [HttpPost]
        public IActionResult InsertOrdenProduccion([FromBody] OrdenProduccionModel nuevaOrdenProduccion)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "INSERT INTO OrdenProduccion (Cantidad, Fecha_Orden, Fecha_Entrega, id_empleado) " +
                               "VALUES (@Cantidad, @Fecha_Orden, @Fecha_Entrega, @id_empleado)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Cantidad", nuevaOrdenProduccion.Cantidad);
                cmd.Parameters.AddWithValue("@Fecha_Orden", nuevaOrdenProduccion.FechaOrden);
                cmd.Parameters.AddWithValue("@Fecha_Entrega", nuevaOrdenProduccion.FechaEntrega);
                cmd.Parameters.AddWithValue("@id_empleado", nuevaOrdenProduccion.ID_Empleado);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Orden de producción insertada correctamente.");
                }
                else
                {
                    return BadRequest("No se pudo insertar la orden de producción.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar orden de producción: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrdenProduccion(int id, [FromBody] OrdenProduccionModel ordenProduccionActualizada)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "UPDATE OrdenProduccion SET Cantidad = @Cantidad, Fecha_Orden = @Fecha_Orden, " +
                               "Fecha_Entrega = @Fecha_Entrega, id_empleado = @id_empleado WHERE ID = @ID";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Cantidad", ordenProduccionActualizada.Cantidad);
                cmd.Parameters.AddWithValue("@Fecha_Orden", ordenProduccionActualizada.FechaOrden);
                cmd.Parameters.AddWithValue("@Fecha_Entrega", ordenProduccionActualizada.FechaEntrega);
                cmd.Parameters.AddWithValue("@id_empleado", ordenProduccionActualizada.ID_Empleado);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Orden de producción actualizada correctamente.");
                }
                else
                {
                    return NotFound($"Orden de producción con ID {id} no encontrada.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar orden de producción: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }

    }
}
