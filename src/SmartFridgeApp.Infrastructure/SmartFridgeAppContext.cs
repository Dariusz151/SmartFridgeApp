using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.ValueObjects;
using SmartFridgeApp.Infrastructure.FoodProducts;
using SmartFridgeApp.Infrastructure.FridgeItems;
using SmartFridgeApp.Infrastructure.FridgeMembers;
using SmartFridgeApp.Infrastructure.Fridges;
using SmartFridgeApp.Infrastructure.Outbox;
using SmartFridgeApp.Infrastructure.Recipes;
using SmartFridgeApp.Shared.Outbox;

namespace SmartFridgeApp.Infrastructure
{
    public class SmartFridgeAppContext : DbContext
    {
        public DbSet<Fridge> Fridges { get; set; }
        public DbSet<FridgeMember> FridgeMembers { get; set; }
        public DbSet<FridgeItem> FridgeItems { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeCategory> RecipeCategories { get; set; }
        public DbSet<FoodProduct> FoodProducts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public SmartFridgeAppContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FridgeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FridgeMemberEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FridgeItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RecipeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FoodProductEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RecipeCategoryEntityTypeConfiguration());
        }
    }
}
