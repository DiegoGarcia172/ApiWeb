using System;
using System.Data.SqlClient;
namespace WEBAPI.data
{
    public class Conexion
    {
        private readonly string _connectionString;
        public Conexion()
        {
            _connectionString = "Server=DIEGO\\SQLEXPRESS;Database=ProduccionDB;User Id=ra;Password=1234;";
        }
        public SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();
                Console.WriteLine("Conexión establecida correctamente.");
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar: {ex.Message}");
                throw;
            }
        }
        public void CloseConnection(SqlConnection connection)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                Console.WriteLine("Conexión cerrada.");
            }
        }
    }
}
