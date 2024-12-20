using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProyectoPrueba.Models;
using ProyectoPrueba.Services;
using System.Web;

namespace ProyectoPrueba.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseConnection _db = new DatabaseConnection();

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT UserId, Username FROM Users WHERE Username = @user AND Password = @pass", conn);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", password);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Guardar datos en la sesión
                        HttpContext.Session.SetInt32("UserId", reader.GetInt32(0));
                        HttpContext.Session.SetString("Username", reader.GetString(1));
                        return RedirectToAction("Index", "Products");
                    }
                }
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }

        public IActionResult Logout()
        {
            // Limpiar la sesión
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Debe llenar todos los campos.";
                return View();
            }

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO Users (Username, Password) VALUES (@user, @pass)", conn);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", password);

                try
                {
                    cmd.ExecuteNonQuery();
                    ViewBag.Success = "Usuario registrado correctamente.";
                    return RedirectToAction("Login");
                }
                catch (SqlException ex)
                {
                    ViewBag.Error = "Error al registrar el usuario: " + ex.Message;
                    return View();
                }
            }
        }
    }
}
