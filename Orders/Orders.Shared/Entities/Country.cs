using Orders.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Orders.Shared.Entities
{
	public class Country : IEntityWithName
    {
		public int Id { get; set; } // Id del pais

		[Display(Name = "País")] // Nombre del campo que el usuario ve
		[MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")] // Mensaje de error
		[Required(ErrorMessage = "El campo {0} es obligatorio.")] // Si el campo esta vacio
		public string Name { get; set; } = null!; // Campo nombre

        //Relations
        public ICollection<State>? States { get; set; }
        [Display(Name = "Estados/Departamentos")]

        public int StatesNumber => States == null || States.Count == 0 ? 0 : States.Count;
    }
}