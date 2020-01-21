using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.Infrastructure.RecipeFoodProducts
{
    internal class RecipeFoodProductEntityTypeConfiguration : IEntityTypeConfiguration<RecipeFoodProduct>
    {
        public void Configure(EntityTypeBuilder<RecipeFoodProduct> builder)
        {
            builder.HasKey(t => new { t.FoodProductId, t.RecipeId });

            builder.HasOne(rfp => rfp.Recipe)
                .WithMany(p => p.RecipeFoodProducts)
                .HasForeignKey(pi => pi.RecipeId);

            builder.HasOne(rfp => rfp.FoodProduct)
                .WithMany(p => p.RecipeFoodProducts)
                .HasForeignKey(pi => pi.FoodProductId);
        }
    }
}
