using Orders.Shared.DTOs;
using Orders.Shared.Responses;

namespace Orders.Backend.UnitsOfWork.Interfaces
{
	public interface IGenericUnitOfWork<T> where T : class
	{
		Task<ActionResponse<IEnumerable<T>>> GetAsync(); // Traer lista de registros

        Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

        Task<ActionResponse<T>> AddAsync(T model); // Agregar registro

		Task<ActionResponse<T>> UpdateAsync(T model); // Actualizar registro

		Task<ActionResponse<T>> DeleteAsync(int id); // Eliminar registro

		Task<ActionResponse<T>> GetAsync(int id); // Traer registro

	}
}
