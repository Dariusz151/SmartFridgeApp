using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Domain.Models;

namespace SmartFridgeApp.Persistence
{
    public class SmartFridgeContext : DbContext
    {
        public SmartFridgeContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<ConsumedFoodItem> ConsumedFoodItems { get; set; }
    }
}
