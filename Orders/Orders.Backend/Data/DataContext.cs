using Microsoft.EntityFrameworkCore;
using Orders.Shared.Entites;

namespace Orders.Backend.Data
{
    public class DataContext : DbContext
    {
        //ctor
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        //prop -> Agregar referencia a Shared
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
        }
    }
}