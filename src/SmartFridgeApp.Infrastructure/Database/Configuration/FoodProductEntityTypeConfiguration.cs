using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.ValueObjects;
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

            builder.Property(fp => fp.InsertedAt)
                .HasColumnName("InsertedAt")
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.Property(fp => fp.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder.Property(fp => fp.DefaultStorageLocation)
                .HasColumnName("DefaultStorageLocation")
                .IsRequired(false);

            builder.Property(fp => fp.DefaultUnit)
                .HasColumnName("DefaultUnit")
                .IsRequired(false);

            builder.HasOne(fp => fp.Category)
                .WithMany()
                .HasForeignKey("CategoryId");
        }
    }
}
