using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Interfaces
{
	public interface ICountriesRepository
	{
		Task<ActionResponse<Country>> GetAsync(int id); // Traer registro
		Task<ActionResponse<IEnumerable<Country>>> GetAsync(); // Traer lista de registros

		Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination);

		Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
	}
}
