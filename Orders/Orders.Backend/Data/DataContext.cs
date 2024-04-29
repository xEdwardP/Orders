using Microsoft.EntityFrameworkCore;
using Orders.Shared.Entities;

namespace Orders.Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

		// Propiedades
		public DbSet<Category> Categories { get; set; }
		public DbSet<Country> Countries { get; set; }

        // Indices -> Para que no se repitan nombres
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
			modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
		}
    }
}