using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Domain.Fridges;
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
            });

        }
    }
}
