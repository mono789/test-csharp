using Microsoft.Data.SqlClient;

namespace ProyectoPrueba.Services
{
    public class DatabaseConnection
    {
        private readonly string connectionString = "Server=localhost;Database=ProyectoPrueba;Trusted_Connection=True;TrustServerCertificate=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
