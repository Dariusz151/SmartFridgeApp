using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Infrastructure.Database;

namespace SmartFridgeApp.Infrastructure.FridgeMembers;

internal class FridgeMemberEntityTypeConfiguration : IEntityTypeConfiguration<FridgeMember>
{
    public void Configure(EntityTypeBuilder<FridgeMember> builder)
    {
        builder.ToTable("FridgeMembers", SchemaNames.Application);

        builder.HasKey(fm => fm.Id);
        builder.Property(fm => fm.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(fm => fm.FridgeId)
            .HasColumnName("FridgeId")
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

        builder.HasIndex(fm => new { fm.FridgeId, fm.Email }).IsUnique();
    }
}
