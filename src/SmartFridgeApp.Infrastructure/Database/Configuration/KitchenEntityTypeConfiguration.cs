using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Infrastructure.Database;

namespace SmartFridgeApp.Infrastructure.Kitchens
{
    internal class KitchenEntityTypeConfiguration : IEntityTypeConfiguration<Kitchen>
    {
        public void Configure(EntityTypeBuilder<Kitchen> builder)
        {
            builder.ToTable("Kitchens", SchemaNames.Application);
            builder.HasKey(b => b.Id);

            builder.Property("Name").HasColumnName("Name")
                .IsRequired().HasMaxLength(50);
            builder.Property("Address").HasColumnName("Address")
                .HasMaxLength(100);
            builder.Property("Desc").HasColumnName("Desc")
                .HasMaxLength(250);

            builder.Property(b => b.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.Ignore(b => b.DomainEvents);

            builder.HasMany(b => b.Members)
                .WithOne()
                .HasForeignKey(fm => fm.KitchenId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(Kitchen.Members))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
