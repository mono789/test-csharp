using Microsoft.Data.SqlClient;
using ProyectoPrueba.Models;

namespace ProyectoPrueba.Services
{
    public class AuthService
    {
        private readonly DatabaseConnection _db;

        public AuthService()
        {
            _db = new DatabaseConnection();
        }

        public User ValidateUser(string username, string password)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "SELECT UserId, Username, IsActive FROM Users WHERE Username = @Username AND Password = @Password",
                    connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password); // En producción usar hash

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                IsActive = reader.GetBoolean(2)
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
