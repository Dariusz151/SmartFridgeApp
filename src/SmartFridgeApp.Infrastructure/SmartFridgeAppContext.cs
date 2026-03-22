using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.ValueObjects;
using SmartFridgeApp.Infrastructure.FoodProducts;
using SmartFridgeApp.Infrastructure.KitchenMembers;
using SmartFridgeApp.Infrastructure.Kitchens;
using SmartFridgeApp.Infrastructure.Outbox;
using SmartFridgeApp.Infrastructure.Recipes;
using SmartFridgeApp.Shared.Outbox;

namespace SmartFridgeApp.Infrastructure
{
    public class SmartFridgeAppContext : DbContext
    {
        public DbSet<Kitchen> Kitchens { get; set; }
        public DbSet<KitchenMember> KitchenMembers { get; set; }
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
            modelBuilder.ApplyConfiguration(new KitchenEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new KitchenMemberEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RecipeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FoodProductEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RecipeCategoryEntityTypeConfiguration());
        }
    }
}
