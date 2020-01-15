﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Infrastructure.Database;

namespace SmartFridgeApp.Infrastructure.FoodProducts
{
    internal class FoodProductEntityTypeConfiguration : IEntityTypeConfiguration<FoodProduct>
    {
        public void Configure(EntityTypeBuilder<FoodProduct> builder)
        {
            builder.ToTable("FoodProducts", SchemaNames.Application);
            builder.HasKey(b => b.FoodProductId);
            builder.Property("Name").HasColumnName("Name");
        }
    }
}
