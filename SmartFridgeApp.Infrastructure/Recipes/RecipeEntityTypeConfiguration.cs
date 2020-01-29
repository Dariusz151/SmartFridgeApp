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
            builder.Property("Name").HasColumnName("Name");
            builder.Property("Description").HasColumnName("Description");
            builder.Property("DifficultyLevel").HasColumnName("DifficultyLevel");
            builder.Property("MinutesRequired").HasColumnName("MinutesRequired");
            builder.Property("Category").HasColumnName("Category");

            var converter = new ValueConverter<List<FoodProduct>, string>(v=>v.XmlSerializeToString().ToString(), v=>v.XmlDeserializeFromString<List<FoodProduct>>());
            
            builder.Property("FoodProducts").HasColumnName("FoodProducts").HasConversion(converter);
        }
    }
}
