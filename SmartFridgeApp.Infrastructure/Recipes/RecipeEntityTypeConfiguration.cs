using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Domain.Recipes;

namespace SmartFridgeApp.Infrastructure.Recipes
{
    internal class RecipeEntityTypeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasKey(b => b.Id);
        }
    }
}
