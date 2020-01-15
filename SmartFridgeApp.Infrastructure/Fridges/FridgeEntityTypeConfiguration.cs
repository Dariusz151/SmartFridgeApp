using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.Fridges;
using SmartFridgeApp.Domain.Shared;
using SmartFridgeApp.Domain.Users;
using SmartFridgeApp.Infrastructure.Database;

namespace SmartFridgeApp.Infrastructure.Fridges
{
    internal class FridgeEntityTypeConfiguration : IEntityTypeConfiguration<Fridge>
    {
        public void Configure(EntityTypeBuilder<Fridge> builder)
        {
            builder.ToTable("Fridges", SchemaNames.Application);
            builder.HasKey(b => b.Id);

            builder.Property("Name").HasColumnName("Name");
            builder.Property("Address").HasColumnName("Address");
            builder.Property("Desc").HasColumnName("Desc");

            builder.OwnsMany<User>("_users", x =>
            {
                x.ToTable("Users", SchemaNames.Application);
                x.HasKey(u => u.Id);
                x.HasForeignKey("FridgeId");

                x.Property("Name").HasColumnName("Name");
                x.Property("Email").HasColumnName("Email");
                x.Property("_createdAt").HasColumnName("CreatedAt");
                x.Property("_welcomeEmailWasSent").HasColumnName("WelcomeEmailWasSent");
                
                x.OwnsMany<FridgeItem>("_fridgeItems", f =>
                {
                    f.ToTable("FridgeItems");
                    f.HasKey(k => k.Id);
                    f.HasForeignKey("UserId");

                    f.Property("Desc").HasColumnName("Desc");
                    f.Property<DateTime>("ExpirationDate").HasColumnName("ExpirationDate");
                    f.Property<DateTime>("EnteredAt").HasColumnName("EnteredAt");
                    f.Property("IsConsumed").HasColumnName("IsConsumed");
                    f.Property(cat=>cat.Category).HasConversion(conv => conv.ToString(),
                        c => (Category)Enum.Parse(typeof(Category), c));

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
