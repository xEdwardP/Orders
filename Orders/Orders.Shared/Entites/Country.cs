using System.ComponentModel.DataAnnotations; //Libreria necesaria para los DataAnnotations

namespace Orders.Shared.Entites
{
    public class Country
    {
        public int Id { get; set; } // Id del pais

        [Display(Name = "País")] // Nombre del campo que el usuario ve
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")] // Mensaje de error
        [Required(ErrorMessage = "El campo {0} es obligatorio.")] // Si el campo esta vacio
        public string Name { get; set; } = null!; // Campo nombre
    }
}