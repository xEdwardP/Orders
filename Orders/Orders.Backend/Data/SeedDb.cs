using Orders.Shared.Entities;

namespace Orders.Backend.Data
{
	public class SeedDb
	{
		private readonly DataContext _context;

		public SeedDb(DataContext context)
        {
			_context = context;
		}

		public async Task SeedAsync()
		{
			await _context.Database.EnsureCreatedAsync();
			await CheckCountriesAsync();
			await CheckCategoriesAsync();
		}

		// Verifica si existen datos en la tabla de Categories
		private async Task CheckCategoriesAsync()
		{
			if (!_context.Categories.Any())
			{
				_context.Categories.Add(new Category { Name = "Categoria 1" });
				_context.Categories.Add(new Category { Name = "Categoria 2" });
				_context.Categories.Add(new Category { Name = "Categoria 3" });
				_context.Categories.Add(new Category { Name = "Categoria 4" });
				_context.Categories.Add(new Category { Name = "Categoria 5" });
				await _context.SaveChangesAsync();
			}
		}

		// Verifica si existen datos en la tabla de Countries
		private async Task CheckCountriesAsync()
		{
			if(!_context.Countries.Any())
			{
				_context.Countries.Add(new Country { Name = "Costa Rica" });
				_context.Countries.Add(new Country { Name = "El Salvador" });
				_context.Countries.Add(new Country { Name = "Guatemala" });
				_context.Countries.Add(new Country { Name = "Honduras " });
				_context.Countries.Add(new Country { Name = "Nicaragua " });
				_context.Countries.Add(new Country { Name = "Panamá" });
				await _context.SaveChangesAsync();
			}
		}
	}
}
