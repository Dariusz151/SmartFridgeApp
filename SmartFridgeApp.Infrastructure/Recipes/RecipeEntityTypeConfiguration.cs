using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.Recipes;
using SmartFridgeApp.Infrastructure.Database;
using SmartFridgeApp.Infrastructure.Extensions;

namespace SmartFridgeApp.Infrastructure.Recipes
{
    internal class RecipeEntityTypeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("Recipes", SchemaNames.Application);
            builder.HasKey(b => b.RecipeId);
            builder.Property("Name").HasColumnName("Name")
                .IsRequired().HasMaxLength(100);
            builder.Property("Description").HasColumnName("Description").HasMaxLength(5000);
            builder.Property("RequiredTime").HasColumnName("RequiredTime");
            builder.Property("LevelOfDifficulty").HasColumnName("LevelOfDifficulty");

            var converter = new ValueConverter<List<FoodProductDetails>, string>(v=>v.XmlSerializeToString().ToString(), v=>v.XmlDeserializeFromString<List<FoodProductDetails>>());
            
            builder.Property("FoodProducts").HasColumnName("FoodProducts").HasConversion(converter);

            builder.HasOne<RecipeCategory>(c => c.RecipeCategory);
        }
    }
}
