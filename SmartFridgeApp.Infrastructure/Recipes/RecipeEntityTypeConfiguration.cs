using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Domain.Recipes;
using SmartFridgeApp.Infrastructure.Database;

namespace SmartFridgeApp.Infrastructure.Recipes
{
    internal class RecipeEntityTypeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("Recipes", SchemaNames.Application);
            builder.HasKey(b => b.RecipeId);
            builder.Property("Name").HasColumnName("Name");

            

            //builder.HasMany<FoodProduct>(fp => fp.Products);
        }
    }
}
