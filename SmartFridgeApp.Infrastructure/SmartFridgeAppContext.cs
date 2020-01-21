using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.Fridges;
using SmartFridgeApp.Domain.RecipeFoodProducts;
using SmartFridgeApp.Domain.Recipes;
using SmartFridgeApp.Domain.Users;
using SmartFridgeApp.Infrastructure.FoodProducts;
using SmartFridgeApp.Infrastructure.Fridges;
using SmartFridgeApp.Infrastructure.RecipeFoodProducts;
using SmartFridgeApp.Infrastructure.Recipes;

namespace SmartFridgeApp.Infrastructure
{
    public class SmartFridgeAppContext : DbContext
    {
        public DbSet<Fridge> Fridges { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FridgeItem> FridgeItems { get; set; }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<FoodProduct> FoodProducts { get; set; }
        //public DbSet<RecipeFoodProduct> RecipeFoodProducts { get; set; }
        
        public SmartFridgeAppContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<RecipeFoodProduct>()
            //    .HasKey(t => new {t.FoodProductId, t.RecipeId});

            //modelBuilder.Entity<RecipeFoodProduct>()
            //    .HasOne(rfp => rfp.Recipe)
            //    .WithMany(p => p.RecipeFoodProducts)
            //    .HasForeignKey(pi => pi.RecipeId);

            //modelBuilder.Entity<RecipeFoodProduct>()
            //    .HasOne(rfp => rfp.FoodProduct)
            //    .WithMany(p => p.RecipeFoodProducts)
            //    .HasForeignKey(pi => pi.FoodProductId);

            modelBuilder.ApplyConfiguration(new FridgeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RecipeFoodProductEntityTypeConfiguration());
        }
    }
}
