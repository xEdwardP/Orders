using Orders.Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace Orders.Shared.DTOs
{
    public class UserDTO : User
    {
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio!")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres!")]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden!")]
        [Display(Name = "Confirmación de contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es obligatorio!")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres!")]
        public string PasswordConfirm { get; set; } = null!;
    }
}