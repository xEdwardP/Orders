using Microsoft.AspNetCore.Identity;
using Orders.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Orders.Shared.Entities
{
	public class User : IdentityUser
	{
		[Display(Name = "Documento")]
		[MaxLength(20, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres!")]
		[Required(ErrorMessage = "El campo {0} es obligatorio!")]
		public string Document { get; set; } = null!;

		[Display(Name = "Nombres")]
		[MaxLength(150, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres!")]
		[Required(ErrorMessage = "El campo {0} es obligatorio!")]
		public string FirstName { get; set; } = null!;

		[Display(Name = "Apellidos")]
		[MaxLength(150, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres!")]
		[Required(ErrorMessage = "El campo {0} es obligatorio!")]
		public string LastName { get; set; } = null!;

		[Display(Name = "Direccion")]
		[MaxLength(200, ErrorMessage = "EL campo {0} debe tener maximo {1} caracteres")]
		[Required(ErrorMessage = "El campo {0} es obligatorio!")]
		public string Address { get; set; } = null!;

		[Display(Name = "Foto")]
		public string? UserPhoto { get; set; }

		[Display(Name = "Tipo de Usuario")]
		public UserType UserType { get; set; }

		public City? City { get; set; }

		[Display(Name = "Ciudad")]
		[Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una {0}!")]
		public int CityId { get; set; }

		[Display(Name = "Usuario")]
		public string FullName => $"{FirstName}{LastName}";
	}
}