using System;
using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.Fridges;
using SmartFridgeApp.Domain.Users;
using SmartFridgeApp.Infrastructure.Fridges;

namespace SmartFridgeApp.Infrastructure
{
    public class SmartFridgeAppContext : DbContext
    {
        public DbSet<Fridge> Fridges { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FridgeItem> FridgeItems { get; set; }

        public SmartFridgeAppContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FridgeEntityTypeConfiguration());
        }
    }
}
