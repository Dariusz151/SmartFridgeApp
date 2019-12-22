using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.Fridges;
using SmartFridgeApp.Domain.Shared;
using SmartFridgeApp.Domain.Users;

namespace SmartFridgeApp.Infrastructure.Fridges
{
    internal class FridgeEntityTypeConfiguration : IEntityTypeConfiguration<Fridge>
    {
        public void Configure(EntityTypeBuilder<Fridge> builder)
        {
            builder.HasKey(b => b.Id);
            builder.OwnsMany<User>("_users", x =>
            {
                x.ToTable("Users");
                x.HasForeignKey("FridgeId");

                x.HasKey(u => u.Id);

                x.OwnsMany<FridgeItem>("_fridgeItems", f =>
                {
                    f.ToTable("FridgeItems");
                    f.HasForeignKey("UserId");
                    f.HasKey(k => k.Id);
                    
                    f.Property(cat=>cat.Category).HasConversion(conv => conv.ToString(),
                        c => (Category)Enum.Parse(typeof(Category), c));

                    f.OwnsOne<AmountValue>("AmountValue", av =>
                    {
                        av.Property(p => p.Value).HasColumnName("Value");
                        av.Property(p => p.Unit).HasColumnName("Unit");
                    });
                });
            });

        }
    }
}
