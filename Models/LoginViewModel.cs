using System.ComponentModel.DataAnnotations;

namespace ProyectoPrueba.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        [Display(Name = "Usuario")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
