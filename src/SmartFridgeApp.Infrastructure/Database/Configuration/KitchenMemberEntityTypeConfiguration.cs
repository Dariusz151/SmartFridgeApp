using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Infrastructure.Database;

namespace SmartFridgeApp.Infrastructure.KitchenMembers;

internal class KitchenMemberEntityTypeConfiguration : IEntityTypeConfiguration<KitchenMember>
{
    public void Configure(EntityTypeBuilder<KitchenMember> builder)
    {
        builder.ToTable("KitchenMembers", SchemaNames.Application);

        builder.HasKey(fm => fm.Id);
        builder.Property(fm => fm.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(fm => fm.kitchenId)
            .HasColumnName("kitchenId")
            .IsRequired();

        builder.Property(fm => fm.Email)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(fm => fm.MemberRole)
            .HasColumnName("MemberRole")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(fm => fm.Status)
            .HasColumnName("Status")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(fm => fm.Color)
            .HasColumnName("Color")
            .IsRequired()
            .HasMaxLength(7);

        builder.Property(fm => fm.InvitedAt)
            .HasColumnName("InvitedAt")
            .IsRequired();

        builder.HasIndex(fm => new { fm.kitchenId, fm.Email }).IsUnique();
    }
}
