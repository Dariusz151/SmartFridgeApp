using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Infrastructure.Database;

namespace SmartFridgeApp.Infrastructure.FoodProducts;

internal class ProductVariantEntityTypeConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("ProductVariants", SchemaNames.Application);
        builder.HasKey(v => v.VariantId);
        builder.Property(v => v.VariantId).HasColumnName("VariantId").ValueGeneratedOnAdd();
        builder.Property(v => v.Name).HasColumnName("Name").IsRequired().HasMaxLength(80);
        builder.Property(v => v.Barcode).HasColumnName("Barcode").HasMaxLength(50);
        builder.Property(v => v.FoodProductId).HasColumnName("FoodProductId");

        builder.HasOne(v => v.FoodProduct)
            .WithMany(fp => fp.Variants)
            .HasForeignKey(v => v.FoodProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(v => v.Barcode)
            .IsUnique()
            .HasFilter("\"Barcode\" IS NOT NULL");
    }
}
