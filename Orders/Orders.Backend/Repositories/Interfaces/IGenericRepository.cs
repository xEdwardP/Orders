﻿using Orders.Shared.DTOs;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Interfaces
{
	public interface IGenericRepository<T> where T : class
	{
		Task<ActionResponse<T>> GetAsync(int id); // Traer registro

		Task<ActionResponse<IEnumerable<T>>> GetAsync(); // Traer lista de registros

        Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination); // 

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination); // Cant Pages

        Task<ActionResponse<T>> AddAsync(T entity); // Agregar registro

		Task<ActionResponse<T>> DeleteAsync(int id); // Eliminar registro

		Task<ActionResponse<T>> UpdateAsync(T entity); // Actualizar registro
    }
}