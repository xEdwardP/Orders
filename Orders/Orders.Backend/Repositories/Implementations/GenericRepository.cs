﻿using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implementations
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly DataContext _context;
		private readonly DbSet<T> _entity;

		public GenericRepository(DataContext context)
        {
			_context = context;
			_entity = _context.Set<T>();
		}

        public virtual async Task<ActionResponse<T>> AddAsync(T entity)
		{
			_context.Add(entity);
			try
			{
				await _context.SaveChangesAsync();
				return new ActionResponse<T>
				{
					WasSuccess = true,
					Result = entity
				};
			}
			catch (DbUpdateException)
			{
				return DbUpdateExceptionActionResponse();
			}
			catch(Exception exception)
			{
				return ExceptionActionResponse(exception);
			}
		}

		public virtual async Task<ActionResponse<T>> DeleteAsync(int id)
		{
			var row = await _entity.FindAsync(id);
			if(row == null)
			{
				return new ActionResponse<T>
				{
					WasSuccess = false,
					Message = "REGISTRO NO ENCONTRADO!"
				};
			}

			try
			{
				_entity.Remove(row);
				await _context.SaveChangesAsync();
				return new ActionResponse<T> { WasSuccess = true };
			}
			catch
			{
				return new ActionResponse<T>
				{
					WasSuccess = false,
					Message = "EL REGISTRO NO SE PUEDE ELIMINAR PORQUE TIENE REGISTROS RELACIONADOS!"
				};
			}
		}

		public virtual async Task<ActionResponse<T>> GetAsync(int id)
		{
			var row = await _entity.FindAsync(id);
			if (row == null)
			{
				return new ActionResponse<T>
				{
					WasSuccess = false,
					Message = "REGISTRO NO ENCONTRADO!"
				};
			}
			return new ActionResponse<T>
			{
				WasSuccess = true,
				Result = row
			};
		}

		public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()
		{
			return new ActionResponse<IEnumerable<T>>
			{
				WasSuccess = true,
				Result = await _entity.ToListAsync()
			};
		}

		public virtual async Task<ActionResponse<T>> UpdateAsync(T entity)
		{
			_context.Update(entity);
			try
			{
				await _context.SaveChangesAsync();
				return new ActionResponse<T>
				{
					WasSuccess = true,
					Result = entity
				};
			}
			catch (DbUpdateException)
			{
				return DbUpdateExceptionActionResponse();
			}
			catch (Exception exception)
			{
				return ExceptionActionResponse(exception);
			}
		}

		private ActionResponse<T> DbUpdateExceptionActionResponse()
		{
			return new ActionResponse<T>
			{
				WasSuccess = true,
				Message = "YA EXISTE UN REGISTRO SIMILAR EN LA BASE DE DATOS!"
			};
		}

		private ActionResponse<T> ExceptionActionResponse(Exception exception)
		{
			return new ActionResponse<T>
			{
				WasSuccess = true,
				Message = exception.Message
			};
		}
	}
}
