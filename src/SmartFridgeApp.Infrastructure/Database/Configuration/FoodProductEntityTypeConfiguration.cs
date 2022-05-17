using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Infrastructure.Database;

namespace SmartFridgeApp.Infrastructure.FoodProducts
{
    internal class FoodProductEntityTypeConfiguration : IEntityTypeConfiguration<FoodProduct>
    {
        public void Configure(EntityTypeBuilder<FoodProduct> builder)
        {
            builder.ToTable("FoodProducts", SchemaNames.Application);
            builder.HasKey(b => b.FoodProductId);
            builder.Property("FoodProductId").HasColumnName("FoodProductId").ValueGeneratedOnAdd();
            builder.Property("Name").HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(40);

            builder.HasOne<Category>(c => c.Category);
        }
    }
}
