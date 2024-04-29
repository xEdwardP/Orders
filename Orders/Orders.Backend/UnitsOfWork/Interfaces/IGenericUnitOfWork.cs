using Orders.Shared.Responses;

namespace Orders.Backend.UnitsOfWork.Interfaces
{
	public interface IGenericUnitOfWork<T> where T : class
	{
		Task<ActionResponse<IEnumerable<T>>> GetAsync(); // Traer lista de registros

		Task<ActionResponse<T>> AddAsync(T model); // Agregar registro

		Task<ActionResponse<T>> UpdateAsync(T model); // Actualizar registro

		Task<ActionResponse<T>> DeleteAsync(int id); // Eliminar registro

		Task<ActionResponse<T>> GetAsync(int id); // Traer registro

	}
}
