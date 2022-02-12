using CarsData.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarsData.DAL.Context
{
    public class CarDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=WIN-ML3N7I995PP;Initial Catalog=CarsDb;User ID=sa;Password=asus1212;");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Car>? Cars { get; set; }
    }
}
