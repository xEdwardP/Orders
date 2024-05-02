using Orders.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Orders.Shared.Entities
{
	public class State : IEntityWithName
	{
		public int Id { get; set; }

		[Display(Name = "Departamento / Estado")] // Nombre del campo que el usuario ve
		[MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")] // Mensaje de error
		[Required(ErrorMessage = "El campo {0} es obligatorio.")] // Si el campo esta vacio
		public string Name { get; set; } = null!; // Campo nombre

		//Relations
		public int CountryId { get; set; }
		public Country? Country { get; set; }

		public ICollection<City>? Cities { get; set; }
		[Display(Name = "Ciudades")]
		public int CitiesNumber => Cities == null || Cities.Count == 0 ? 0 : Cities.Count;
	}
}