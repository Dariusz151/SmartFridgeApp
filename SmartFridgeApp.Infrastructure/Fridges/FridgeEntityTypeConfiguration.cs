using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.FridgeItems;
using SmartFridgeApp.Domain.Models.Fridges;
using SmartFridgeApp.Domain.Models.Users;
using SmartFridgeApp.Domain.Shared;
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
            builder.Property("IsWelcomed").HasColumnName("IsWelcomed")
                .HasMaxLength(10);

            builder.OwnsMany<User>("_users", x =>
            {
                x.ToTable("Users", SchemaNames.Application);
                x.HasKey(u => u.Id);
                x.WithOwner()
                    .HasConstraintName("FK_FridgeUsers");

                //x.HasForeignKey("FridgeId");

                x.Property("Name").HasColumnName("Name")
                    .IsRequired()
                    .HasMaxLength(100);
                x.Property("Email").HasColumnName("Email")
                    .IsRequired()
                    .HasMaxLength(250);
                x.Property("_createdAt").HasColumnName("CreatedAt");
                
                x.OwnsMany<FridgeItem>("_fridgeItems", f =>
                {
                    f.ToTable("FridgeItems", SchemaNames.Application);
                    f.HasKey(k => k.Id);
                    f.WithOwner()
                        .HasConstraintName("FK_UsersFridgeItems");

                    f.Property("Note").HasColumnName("Note")
                        .HasMaxLength(1000);
                    f.Property<DateTime>("ExpirationDate").HasColumnName("ExpirationDate");
                    f.Property<DateTime>("EnteredAt").HasColumnName("EnteredAt");
                    f.Property("IsConsumed").HasColumnName("IsConsumed");
                    
                    f.OwnsOne<AmountValue>("AmountValue", av =>
                    {
                        av.Property(p => p.Value).HasColumnName("Value");
                        av.Property(p => p.Unit).HasColumnName("Unit").HasConversion(con => con.ToString(),
                            c => (Unit) Enum.Parse(typeof(Unit), c));
                    });
                });
            });
        }
    }
}
