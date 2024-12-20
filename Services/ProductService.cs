using Microsoft.Data.SqlClient;
using ProyectoPrueba.Models;

namespace ProyectoPrueba.Services
{
    public class ProductService
    {
        private readonly DatabaseConnection _db;
        private readonly int _currentUserId;

        public ProductService(int userId)
        {
            _db = new DatabaseConnection();
            _currentUserId = userId;
        }

        public List<Product> GetAll()
        {
            var products = new List<Product>();
            using (var connection = _db.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "SELECT ProductId, Name, Description, Price, Stock, CreatedBy, CreatedDate FROM Products",
                    connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                Stock = reader.GetInt32(4),
                                CreatedBy = reader.GetInt32(5),
                                CreatedDate = reader.GetDateTime(6)
                            });
                        }
                    }
                }
            }
            return products;
        }

        public Product GetById(int id)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT ProductId, Name, Description, Price, Stock, CreatedBy, CreatedDate FROM Products WHERE ProductId = @ProductId", connection))
                {
                    command.Parameters.AddWithValue("@ProductId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                ProductId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                Stock = reader.GetInt32(4),
                                CreatedBy = reader.GetInt32(5),
                                CreatedDate = reader.GetDateTime(6)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void Create(Product product)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(@"
                INSERT INTO Products (Name, Description, Price, Stock, CreatedBy)
                VALUES (@Name, @Description, @Price, @Stock, @CreatedBy)", connection))
                {
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Description", product.Description);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Stock", product.Stock);
                    command.Parameters.AddWithValue("@CreatedBy", _currentUserId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Product product)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(@"
                UPDATE Products 
                SET Name = @Name, 
                    Description = @Description, 
                    Price = @Price, 
                    Stock = @Stock 
                WHERE ProductId = @ProductId", connection))
                {
                    command.Parameters.AddWithValue("@ProductId", product.ProductId);
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Description", product.Description);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Stock", product.Stock);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = _db.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Products WHERE ProductId = @ProductId", connection))
                {
                    command.Parameters.AddWithValue("@ProductId", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
