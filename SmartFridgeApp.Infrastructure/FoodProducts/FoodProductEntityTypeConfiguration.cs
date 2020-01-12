using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.Shared;
using SmartFridgeApp.Domain.Users;

namespace SmartFridgeApp.Infrastructure.FoodProducts
{
    internal class FoodProductEntityTypeConfiguration : IEntityTypeConfiguration<FoodProduct>
    {
        public void Configure(EntityTypeBuilder<FoodProduct> builder)
        {
            builder.HasKey(b => b.Id);
        }
    }
}
