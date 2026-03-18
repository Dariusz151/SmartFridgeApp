using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Infrastructure.Database;

namespace SmartFridgeApp.Infrastructure.Fridges
{
    internal class FridgeEntityTypeConfiguration : IEntityTypeConfiguration<Fridge>
    {
        public void Configure(EntityTypeBuilder<Fridge> builder)
        {
            builder.ToTable("Fridges", SchemaNames.Application);
            builder.HasKey(b => b.Id);

            builder.Property("Name").HasColumnName("Name")
                .IsRequired().HasMaxLength(50);
            builder.Property("Address").HasColumnName("Address")
                .HasMaxLength(100);
            builder.Property("Desc").HasColumnName("Desc")
                .HasMaxLength(250);
            builder.Property(b => b.WasteScore)
                .HasColumnName("WasteScore")
                .IsRequired()
                .HasDefaultValue(1000);

            builder.Property(b => b.ActiveItemCount)
                .HasColumnName("ActiveItemCount")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(b => b.AverageItemCount)
                .HasColumnName("AverageItemCount")
                .IsRequired()
                .HasDefaultValue(0.0);

            builder.Property(b => b.InventorySampleCount)
                .HasColumnName("InventorySampleCount")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(b => b.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.Property(b => b.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder.Ignore(b => b.DomainEvents);

            builder.HasMany(b => b.Members)
                .WithOne()
                .HasForeignKey(fm => fm.FridgeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(Fridge.Members))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
