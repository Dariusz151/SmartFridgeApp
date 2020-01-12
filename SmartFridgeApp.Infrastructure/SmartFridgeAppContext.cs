using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.Fridges;
using SmartFridgeApp.Domain.Users;
using SmartFridgeApp.Infrastructure.FoodProducts;
using SmartFridgeApp.Infrastructure.Fridges;

namespace SmartFridgeApp.Infrastructure
{
    public class SmartFridgeAppContext : DbContext
    {
        public DbSet<Fridge> Fridges { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FridgeItem> FridgeItems { get; set; }

        public DbSet<FoodProduct> FoodProducts { get; set; }

        public SmartFridgeAppContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FridgeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FoodProductEntityTypeConfiguration());
        }
    }
}
