using Microsoft.EntityFrameworkCore;
using Orders.Backend.Helpers.ImgHelpers;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Enums;
using System.Runtime.InteropServices;

namespace Orders.Backend.Data
{
	public class SeedDb
	{
		private readonly DataContext _context;
		private readonly IUsersUnitOfWork _usersUnitOfWork;
        private readonly IFileStorage _fileStorage;

        public SeedDb(DataContext context, IUsersUnitOfWork usersUnitOfWork, IFileStorage fileStorage)
        {
			_context = context;
			_usersUnitOfWork = usersUnitOfWork;
            _fileStorage = fileStorage;
        }

		public async Task SeedAsync()
		{
			await _context.Database.EnsureCreatedAsync();
            //await CheckCountriesFullAsync();
            await CheckCountriesAsync();
			await CheckCategoriesAsync();
			await CheckRolesAsync();
            await CheckProductsAsync();
            await CheckUserAsync("1010", "Edward", "Pineda", "epineda@yopmail.com", "99887766", "Calle Luna Calle Sol", "Jude.jpg", UserType.Admin);
            await CheckUserAsync("0008", "Hector", "Lavoe", "hector@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "hector.jpg", UserType.User);
        }

        private async Task CheckUsersAsyncA()
        {
            await CheckUserAsync("0002", "Ledys", "Bedoya", "ledys@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "LedysBedoya.jpg", UserType.User);
            await CheckUserAsync("0003", "Brad", "Pitt", "brad@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "Brad.jpg", UserType.User);
            await CheckUserAsync("0004", "Angelina", "Jolie", "angelina@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "Angelina.jpg", UserType.User);
            await CheckUserAsync("0005", "Bob", "Marley", "bob@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "bob.jpg", UserType.User);
            await CheckUserAsync("0006", "Celia", "Cruz", "celia@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "celia.jpg", UserType.Admin);
            await CheckUserAsync("0007", "Fredy", "Mercury", "fredy@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "fredy.jpg", UserType.User);
            //await CheckUserAsync("0008", "Hector", "Lavoe", "hector@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "hector.jpg", UserType.User);
            await CheckUserAsync("0009", "Liv", "Taylor", "liv@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "liv.jpg", UserType.User);
            await CheckUserAsync("0010", "Otep", "Shamaya", "otep@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "otep.jpg", UserType.User);
            await CheckUserAsync("0011", "Ozzy", "Osbourne", "ozzy@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "ozzy.jpg", UserType.User);
            await CheckUserAsync("0012", "Selena", "Quintanilla", "selenba@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "selena.jpg", UserType.User);
        }

        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                await AddProductAsync("Desayunos / Cenas", 90, 100F, new List<string>() { "Tipico", "Economico" }, new List<string>() { "pollofp.jpeg" });
                await AddProductAsync("Pollo Frito con papas", 140, 100F, new List<string>() { "Tipico", "Pollo", "Carnes" }, new List<string>() { "doc.jpeg" });
                await AddProductAsync("Pollo Frito con tajadas", 130, 100F, new List<string>() { "Tipico", "Pollo", "Carnes" }, new List<string>() { "polloft.jpeg" });
                await AddProductAsync("Plato mixto", 180, 100F, new List<string>() { "Carnes" }, new List<string>() { "plato_mixto.jpeg" });
                await AddProductAsync("Pincho mixto", 170, 100F, new List<string>() { "Carnes" }, new List<string>() { "pincho_mixto.jpeg" });
                await AddProductAsync("Platano Frito", 50, 100F, new List<string>() { "Tipico", "Economico" }, new List<string>() { "platano_mixto.jpeg" });
                await AddProductAsync("Tortillas con quesillo", 50, 100F, new List<string>() { "Tipico", "Economico" }, new List<string>() { "tortillas_quesillo.jpeg" });

                await AddProductAsync("Torta tica con papas", 110, 100F, new List<string>() { "Extranjero", "Carnes" }, new List<string>() { "tortatp.jpeg" });
                await AddProductAsync("Taco Mexicano", 100, 100F, new List<string>() { "Extranjero" }, new List<string>() { "taco_mexicano.jpeg" });


                await AddProductAsync("Pupusas Mixtas", 50, 100F, new List<string>() { "Tipico", "Economico" }, new List<string>() { "taco_mexicano.jpeg" });
                await AddProductAsync("Baleadas", 15, 100F, new List<string>() { "Tipico", "Economico" }, new List<string>() { "baleadas.jpeg" });
                //await AddProductAsync("Bolibaleadas", 25, 100F, new List<string>() { "Tipico", "Economico" }, new List<string>() { "bolibaleadas.jpeg" });
                //await AddProductAsync("Hamburguesa de pollo", 160, 100F, new List<string>() { "Pollo", "Comida Rapida", "Carnes" }, new List<string>() { "hpollo.jpeg" });
                //await AddProductAsync("Hamburguesa con papas", 140, 100F, new List<string>() { "Carnes", "Comida Rapida" }, new List<string>() { "hpp.jpeg" });
                //await AddProductAsync("Hamburguesa doble carne", 170, 100F, new List<string>() { "Tipico", "Comida Rapida", "Carnes" }, new List<string>() { "hdc.jpeg" });
                //await AddProductAsync("Pan Sandwich", 120, 100F, new List<string>() { "Emparedados", "Carnes" }, new List<string>() { "pans.jpeg" });
                //await AddProductAsync("Super Pan Supremo", 130, 100F, new List<string>() { "Comida Rapida", "Emparedados", "Carnes" }, new List<string>() { "span_supremo.jpeg" });
                //await AddProductAsync("Sandwich de pollo con papas", 95, 100F, new List<string>() { "Emparedados", "Carnes" }, new List<string>() { "sandwichpp.jpeg" });
                //await AddProductAsync("Sandwich de Jamon y Queso con papas", 90, 100F, new List<string>() { "Emparedados", "Carnes" }, new List<string>() { "sandwichjqp.jpeg" });
                //await AddProductAsync("Club Sandwich con Papas", 90, 100F, new List<string>() { "Emparedados", "Carnes" }, new List<string>() { "csp.jpeg" });
                //await AddProductAsync("Chicken Fingers", 155, 100F, new List<string>() { "Picante", "Pollo", "Carnes" }, new List<string>() { "chicken_finger.jpeg" });
                //await AddProductAsync("Fajitas de Pollo en Salsa Jalapeña", 155, 100F, new List<string>() { "Picante", "Pollo", "Carnes" }, new List<string>() { "fpsj.jpg" });
                //await AddProductAsync("Fajitas de Pollo en Asadas", 150, 100F, new List<string>() { "Asados", "Pollo", "Carnes" }, new List<string>() { "fpa.jpeg" });
                //await AddProductAsync("Fajitas de Res en Salsa Jalapeña", 160, 100F, new List<string>() { "Picante", "Res", "Carnes" }, new List<string>() { "frsj.jpeg" });
                //await AddProductAsync("Filete de Pollo Empanizado / A la plancha", 160, 100F, new List<string>() { "Pollo", "A la Plancha", "Carnes" }, new List<string>() { "fpep.jpeg" });
                //await AddProductAsync("Bistek encebollado / Entomatado", 170, 100F, new List<string>() { "Res", "Carnes" }, new List<string>() { "bee.jpeg" });
                //await AddProductAsync("Lomo de Res a la Plancha", 165, 100F, new List<string>() { "Res", "A la Plancha", "Carnes" }, new List<string>() { "bee.jpeg" });
                //await AddProductAsync("Chuleta Asada", 140, 100F, new List<string>() { "Asados", "Carnes" }, new List<string>() { "chuleta_asada.jpeg" });
                //await AddProductAsync("Milanesa de Pollo", 170, 100F, new List<string>() { "Pollo", "Carnes" }, new List<string>() { "milanesap.jpeg" });
            }
        }

        private async Task CheckProductsAsync1()
        {
            if (!_context.Products.Any())
            {
                await AddProductAsync("Adidas Barracuda", 2000, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "adidas_barracuda.png" });
                await AddProductAsync("Adidas Superstar", 2500, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "Adidas_superstar.png" });
                await AddProductAsync("Aguacate", 20, 500F, new List<string>() { "Comida" }, new List<string>() { "Aguacate1.png", "Aguacate2.png", "Aguacate3.png" });
                await AddProductAsync("AirPods", 10000, 12F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "airpos.png", "airpos2.png" });
                await AddProductAsync("Akai APC40 MKII", 26500, 12F, new List<string>() { "Tecnología" }, new List<string>() { "Akai1.png", "Akai2.png", "Akai3.png" });
                await AddProductAsync("Apple Watch Ultra", 40000, 24F, new List<string>() { "Apple", "Tecnología" }, new List<string>() { "AppleWatchUltra1.png", "AppleWatchUltra2.png" });
                await AddProductAsync("Audifonos Bose", 30000, 12F, new List<string>() { "Tecnología" }, new List<string>() { "audifonos_bose.png" });
                await AddProductAsync("Bicicleta Ribble", 25000, 6F, new List<string>() { "Deportes" }, new List<string>() { "bicicleta_ribble.png" });
                await AddProductAsync("Camisa Cuadros", 1200, 24F, new List<string>() { "Ropa" }, new List<string>() { "camisa_cuadros.png" });
                await AddProductAsync("Casco Bicicleta", 500, 12F, new List<string>() { "Deportes" }, new List<string>() { "casco_bicicleta.png", "casco.png" });
                await AddProductAsync("Gafas deportivas", 200, 24F, new List<string>() { "Deportes" }, new List<string>() { "Gafas1.png", "Gafas2.png", "Gafas3.png" });
                await AddProductAsync("Hamburguesa triple carne", 100, 240F, new List<string>() { "Comida" }, new List<string>() { "Hamburguesa1.png", "Hamburguesa2.png", "Hamburguesa3.png" });
                await AddProductAsync("iPad", 12500, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "ipad.png" });
                await AddProductAsync("iPhone 13", 50000, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "iphone13.png", "iphone13b.png", "iphone13c.png", "iphone13d.png" });
                await AddProductAsync("Johnnie Walker Blue Label 750ml", 3000, 18F, new List<string>() { "Licores" }, new List<string>() { "JohnnieWalker3.png", "JohnnieWalker2.png", "JohnnieWalker1.png" });
                await AddProductAsync("KOOY Disfraz inflable de gallo para montar", 950, 28F, new List<string>() { "Juguetes" }, new List<string>() { "KOOY1.png", "KOOY2.png", "KOOY3.png" });
                await AddProductAsync("Mac Book Pro", 60000, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "mac_book_pro.png" });
                await AddProductAsync("Mancuernas", 350, 12F, new List<string>() { "Deportes" }, new List<string>() { "mancuernas.png" });
                await AddProductAsync("Mascarilla Cara", 250, 100F, new List<string>() { "Belleza" }, new List<string>() { "mascarilla_cara.png" });
                await AddProductAsync("New Balance 530", 1000, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "newbalance530.png" });
                await AddProductAsync("New Balance 565", 1200, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "newbalance565.png" });
                await AddProductAsync("Nike Air", 1800, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "nike_air.png" });
                await AddProductAsync("Nike Zoom", 1800, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "nike_zoom.png" });
                await AddProductAsync("Buso Adidas Mujer", 1600, 12F, new List<string>() { "Ropa", "Deportes" }, new List<string>() { "buso_adidas.png" });
                await AddProductAsync("Suplemento Boots Original", 950, 12F, new List<string>() { "Nutrición" }, new List<string>() { "Boost_Original.png" });
                await AddProductAsync("Whey Protein", 780, 12F, new List<string>() { "Nutrición" }, new List<string>() { "whey_protein.png" });
                await AddProductAsync("Arnes Mascota", 150, 12F, new List<string>() { "Mascotas" }, new List<string>() { "arnes_mascota.png" });
                await AddProductAsync("Cama Mascota", 450, 12F, new List<string>() { "Mascotas" }, new List<string>() { "cama_mascota.png" });
                await AddProductAsync("Teclado Gamer", 950, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "teclado_gamer.png" });
                await AddProductAsync("Ring de Lujo 17", 600, 33F, new List<string>() { "Autos" }, new List<string>() { "Ring1.png", "Ring2.png" });
                await AddProductAsync("Silla Gamer", 4500, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "silla_gamer.png" });
                await AddProductAsync("Mouse Gamer", 950, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "mouse_gamer.png" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task AddProductAsync(string name, decimal price, float stock, List<string> categories, List<string> images)
        {
            Product prodcut = new()
            {
                Description = name,
                Name = name,
                Price = price,
                Stock = stock,
                ProductCategories = new List<ProductCategory>(),
                ProductImages = new List<ProductImage>()
            };

            foreach (var categoryName in categories)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
                if (category != null)
                {
                    prodcut.ProductCategories.Add(new ProductCategory { Category = category });
                }
            }

            foreach (string? image in images)
            {
                //var filePath = $"{Environment.CurrentDirectory}\\Images\\products\\{image}";
                //var fileBytes = File.ReadAllBytes(filePath);
                //var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "products");
                //prodcut.ProductImages.Add(new ProductImage { Image = imagePath });
                string filePath;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    filePath = $"{Environment.CurrentDirectory}\\Images\\products\\{image}";
                }
                else
                {
                    filePath = $"{Environment.CurrentDirectory}/Images/products/{image}";
                }
                var fileBytes = File.ReadAllBytes(filePath);
                var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "products");
                prodcut.ProductImages.Add(new ProductImage { Image = imagePath });
            }

            _context.Products.Add(prodcut);
        }

        private async Task CheckCountriesFullAsync()
        {
			if (!_context.Countries.Any())
			{
                var countriesStatesCitiesSQLScript = File.ReadAllText("Data\\CountriesStatesCities.sql");
                await _context.Database.ExecuteSqlRawAsync(countriesStatesCitiesSQLScript);
            }
        }

        private async Task CheckRolesAsync()
		{
			await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
			await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
		}

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, string image, UserType userType)
        {
            var user = await _usersUnitOfWork.GetUserAsync(email);
            if (user == null)
            {
                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Name == "Medellín");
                city ??= await _context.Cities.FirstOrDefaultAsync();

                //var filePath = $"{Environment.CurrentDirectory}\\Images\\users\\{image}";
                //var fileBytes = File.ReadAllBytes(filePath);
                //var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "users");

                string filePath;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    filePath = $"{Environment.CurrentDirectory}\\Images\\users\\{image}";
                }
                else
                {
                    filePath = $"{Environment.CurrentDirectory}/Images/users/{image}";
                }

                var fileBytes = File.ReadAllBytes(filePath);
                var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "users");

                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = city,
                    UserType = userType,
                    UserPhoto = imagePath,
                };

                await _usersUnitOfWork.AddUserAsync(user, "123456");
                await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());

                var token = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
                await _usersUnitOfWork.ConfirmEmailAsync(user, token);
            }

            return user;
        }

        // Verifica si existen datos en la tabla de Categories
        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "A la plancha" });
                _context.Categories.Add(new Category { Name = "Asados" });
                _context.Categories.Add(new Category { Name = "Bebidas" });
                _context.Categories.Add(new Category { Name = "Carnes" });
                _context.Categories.Add(new Category { Name = "Comida rapida" });
                _context.Categories.Add(new Category { Name = "Comida" });
                _context.Categories.Add(new Category { Name = "Emparedados" });
                _context.Categories.Add(new Category { Name = "Economico" });
                _context.Categories.Add(new Category { Name = "Extranjero" });
                _context.Categories.Add(new Category { Name = "Picante" });
                _context.Categories.Add(new Category { Name = "Pollo" });
                _context.Categories.Add(new Category { Name = "Res" });
                _context.Categories.Add(new Category { Name = "Snacks" });
                _context.Categories.Add(new Category { Name = "Tipico" });
                await _context.SaveChangesAsync();
            }
        }

        // Verifica si existen datos en la tabla de Categories
        private async Task CheckCategoriesAsync1()
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
