using Orders.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Orders.Shared.Entities
{
	public class City : IEntityWithName
	{
		public int Id { get; set; }

		[Display(Name = "Ciudad")] // Nombre del campo que el usuario ve
		[MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")] // Mensaje de error
		[Required(ErrorMessage = "El campo {0} es obligatorio.")] // Si el campo esta vacio
		public string Name { get; set; } = null!;

		//Relations
		public int StateId { get; set; }
		public State? State { get; set; }
	}
}