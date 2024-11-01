using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using WEBAPI.data;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestConnectionController : ControllerBase
    {
        private readonly Conexion _dbConnection;

        public TestConnectionController()
        {
            _dbConnection = new Conexion();
        }

        [HttpGet]
        public IActionResult TestConnection()
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbConnection.GetConnection();
                return Ok("Conexión exitosa a la base de datos.");
            }
            catch
            {
                return StatusCode(500, "Error al conectar a la base de datos.");
            }
            finally
            {
                _dbConnection.CloseConnection(connection);
            }
        }
    }
}
