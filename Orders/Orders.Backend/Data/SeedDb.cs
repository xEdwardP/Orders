﻿using Orders.Shared.Entities;

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
                _context.Categories.Add(new Category { Name = "Apple" });
                _context.Categories.Add(new Category { Name = "Autos" });
                _context.Categories.Add(new Category { Name = "Belleza" });
                _context.Categories.Add(new Category { Name = "Calzado" });
                _context.Categories.Add(new Category { Name = "Comida" });
                _context.Categories.Add(new Category { Name = "Cosmeticos" });
                _context.Categories.Add(new Category { Name = "Deportes" });
                _context.Categories.Add(new Category { Name = "Erótica" });
                _context.Categories.Add(new Category { Name = "Ferreteria" });
                _context.Categories.Add(new Category { Name = "Gamer" });
                _context.Categories.Add(new Category { Name = "Hogar" });
                _context.Categories.Add(new Category { Name = "Jardín" });
                _context.Categories.Add(new Category { Name = "Jugetes" });
                _context.Categories.Add(new Category { Name = "Lenceria" });
                _context.Categories.Add(new Category { Name = "Mascotas" });
                _context.Categories.Add(new Category { Name = "Nutrición" });
                _context.Categories.Add(new Category { Name = "Ropa" });
                _context.Categories.Add(new Category { Name = "Tecnología" });
                await _context.SaveChangesAsync();
            }
        }

        // Verifica si existen datos en la tabla de Countries
        private async Task CheckCountriesAsync()
		{
			if (!_context.Countries.Any())
			{
				_context.Countries.Add(new Country
				{
					Name = "Colombia",
					States = new List<State>()
			{
				new State()
				{
					Name = "Antioquia",
					Cities = new List<City>() {
						new City() { Name = "Medellín" },
						new City() { Name = "Itagüí" },
						new City() { Name = "Envigado" },
						new City() { Name = "Bello" },
						new City() { Name = "Rionegro" },
					}
				},
				new State()
				{
					Name = "Bogotá",
					Cities = new List<City>() {
						new City() { Name = "Usaquen" },
						new City() { Name = "Champinero" },
						new City() { Name = "Santa fe" },
						new City() { Name = "Useme" },
						new City() { Name = "Bosa" },
					}
				},
			}
				});
				_context.Countries.Add(new Country
				{
					Name = "Estados Unidos",
					States = new List<State>()
			{
				new State()
				{
					Name = "Florida",
					Cities = new List<City>() {
						new City() { Name = "Orlando" },
						new City() { Name = "Miami" },
						new City() { Name = "Tampa" },
						new City() { Name = "Fort Lauderdale" },
						new City() { Name = "Key West" },
					}
				},
				new State()
				{
					Name = "Texas",
					Cities = new List<City>() {
						new City() { Name = "Houston" },
						new City() { Name = "San Antonio" },
						new City() { Name = "Dallas" },
						new City() { Name = "Austin" },
						new City() { Name = "El Paso" },
					}
				},
			}
				});
			}

			await _context.SaveChangesAsync();
		}

	}
}
