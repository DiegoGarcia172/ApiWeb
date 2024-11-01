using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using WEBAPI.data;
//listo
namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly Conexion _dbConnection;

        public EmpleadoController()
        {
            _dbConnection = new Conexion();
        }
        [HttpGet]
        public IActionResult GetEmpleados()
        {
            List<object> empleados = new List<object>(); 
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();

                string query = "SELECT * FROM Empleado"; 
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    empleados.Add(new
                    {
                        ID = reader["ID"],
                        Nombre = reader["Nombre"],
                        Apellido_Paterno = reader["Apellido_Paterno"],
                        Apellido_Materno = reader["Apellido_Materno"],
                        Puesto = reader["Puesto"],
                        id_departamento = reader["id_departamento"]
                    });
                }

                return Ok(empleados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener empleados: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetEmpleadoById(int id)
        {
            SqlConnection connection = null;

            try
            {
                connection = _dbConnection.GetConnection();

                string query = "SELECT * FROM Empleado WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);  
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var empleado = new
                    {
                        ID = reader["ID"],
                        Nombre = reader["Nombre"],
                        Apellido_Paterno = reader["Apellido_Paterno"],
                        Apellido_Materno = reader["Apellido_Materno"],
                        Puesto = reader["Puesto"],
                        id_departamento = reader["id_departamento"]
                    };

                    return Ok(empleado); 
                }
                else
                {
                    return NotFound($"Empleado con ID {id} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener empleado: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        public class EmpleadoModel
        {
            public string Nombre { get; set; }
            public string Apellido_Paterno { get; set; }
            public string Apellido_Materno { get; set; }
            public string Puesto { get; set; }
            public int id_departamento { get; set; }
        }
        [HttpPost]
        public IActionResult InsertEmpleado([FromBody] EmpleadoModel nuevoEmpleado)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "INSERT INTO Empleado (Nombre, Apellido_Paterno, Apellido_Materno, Puesto, id_departamento) " +
                               "VALUES (@Nombre, @Apellido_Paterno, @Apellido_Materno, @Puesto, @id_departamento)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", nuevoEmpleado.Nombre);
                cmd.Parameters.AddWithValue("@Apellido_Paterno", nuevoEmpleado.Apellido_Paterno);
                cmd.Parameters.AddWithValue("@Apellido_Materno", nuevoEmpleado.Apellido_Materno);
                cmd.Parameters.AddWithValue("@Puesto", nuevoEmpleado.Puesto);
                cmd.Parameters.AddWithValue("@id_departamento", nuevoEmpleado.id_departamento);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Empleado insertado correctamente.");
                }
                else
                {
                    return BadRequest("No se pudo insertar el empleado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar empleado: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateEmpleado(int id, [FromBody] EmpleadoModel empleadoActualizado)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                string query = "UPDATE Empleado SET Nombre = @Nombre, Apellido_Paterno = @Apellido_Paterno, " +
                               "Apellido_Materno = @Apellido_Materno, Puesto = @Puesto, id_departamento = @id_departamento " +
                               "WHERE ID = @ID";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Nombre", empleadoActualizado.Nombre);
                cmd.Parameters.AddWithValue("@Apellido_Paterno", empleadoActualizado.Apellido_Paterno);
                cmd.Parameters.AddWithValue("@Apellido_Materno", empleadoActualizado.Apellido_Materno);
                cmd.Parameters.AddWithValue("@Puesto", empleadoActualizado.Puesto);
                cmd.Parameters.AddWithValue("@id_departamento", empleadoActualizado.id_departamento);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Empleado actualizado correctamente.");
                }
                else
                {
                    return NotFound($"Empleado con ID {id} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar empleado: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }

    }
}
