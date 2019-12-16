using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Domain.Fridges;

namespace SmartFridgeApp.Infrastructure.Fridges
{
    internal class FridgeEntityTypeConfiguration : IEntityTypeConfiguration<Fridge>
    {
        public void Configure(EntityTypeBuilder<Fridge> builder)
        {
            builder.HasKey(b => b.Id);

        }
    }
}
