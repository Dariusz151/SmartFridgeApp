using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Infrastructure.Database;

namespace SmartFridgeApp.Infrastructure.FridgeItems;

internal class FridgeItemEntityTypeConfiguration : IEntityTypeConfiguration<FridgeItem>
{
    public void Configure(EntityTypeBuilder<FridgeItem> builder)
    {
        builder.ToTable("FridgeItems", SchemaNames.Application);

        builder.HasKey(fi => fi.Id);
        builder.Property(fi => fi.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(fi => fi.FoodProductId)
            .HasColumnName("FoodProductId")
            .IsRequired();

        builder.Property(fi => fi.MemberId)
            .HasColumnName("MemberId")
            .IsRequired();

        builder.Property(fi => fi.Note)
            .HasColumnName("Note")
            .HasMaxLength(1000);

        builder.Property(fi => fi.ExpirationDate)
            .HasColumnName("ExpirationDate")
            .IsRequired();

        builder.Property(fi => fi.EnteredAt)
            .HasColumnName("EnteredAt")
            .IsRequired();

        builder.Property(fi => fi.IsConsumed)
            .HasColumnName("IsConsumed")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(fi => fi.IsWasted)
            .HasColumnName("IsWasted")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(fi => fi.WastedAt)
            .HasColumnName("WastedAt");

        builder.Property(fi => fi.WasteReason)
            .HasColumnName("WasteReason")
            .HasMaxLength(500);

        // AmountValue is stored as two columns: Value and Unit
        builder.OwnsOne(fi => fi.AmountValue, av =>
        {
            av.Property(a => a.Value)
                .HasColumnName("Value")
                .HasColumnType("numeric")
                .IsRequired();

            av.Property(a => a.Unit)
                .HasColumnName("Unit")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.HasOne(fi => fi.FoodProduct)
            .WithMany()
            .HasForeignKey(fi => fi.FoodProductId);

        builder.Ignore(fi => fi.DomainEvents);
    }
}
