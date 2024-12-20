using System.ComponentModel.DataAnnotations;

namespace ProyectoPrueba.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [Display(Name = "Precio")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "El stock es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser mayor o igual a 0")]
        [Display(Name = "Stock")]
        public int Stock { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
