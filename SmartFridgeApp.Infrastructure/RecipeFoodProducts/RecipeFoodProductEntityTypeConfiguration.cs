using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Domain.RecipeFoodProducts;
using SmartFridgeApp.Infrastructure.Database;

namespace SmartFridgeApp.Infrastructure.RecipeFoodProducts
{
    internal class RecipeFoodProductEntityTypeConfiguration : IEntityTypeConfiguration<RecipeFoodProduct>
    {
        public void Configure(EntityTypeBuilder<RecipeFoodProduct> builder)
        {
            builder.ToTable("RecipeFoodProducts", SchemaNames.Application);
            builder.HasKey(rfp => new {rfp.FoodProductId, rfp.RecipeId});
        }
    }
}
