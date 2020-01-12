using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Domain.FoodProducts;

namespace SmartFridgeApp.Infrastructure.FoodProducts
{
    internal class FoodProductEntityTypeConfiguration : IEntityTypeConfiguration<FoodProduct>
    {
        public void Configure(EntityTypeBuilder<FoodProduct> builder)
        {
            builder.HasKey(b => b.Id);
        }
    }
}
