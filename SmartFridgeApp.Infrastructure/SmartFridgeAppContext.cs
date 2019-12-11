using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Domain.Fridges;

namespace SmartFridgeApp.Infrastructure
{
    public class SmartFridgeAppContext : DbContext
    {
        public DbSet<Fridge> Fridges { get; set; }

        public SmartFridgeAppContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(TypeConfiguration);
        }
    }
}
