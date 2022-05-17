using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Infrastructure.Database;

namespace SmartFridgeApp.Infrastructure.Recipes
{
    internal class RecipeCategoryEntityTypeConfiguration : IEntityTypeConfiguration<RecipeCategory>
    {
        public void Configure(EntityTypeBuilder<RecipeCategory> builder)
        {
            builder.ToTable("RecipeCategories", SchemaNames.Application);
            builder.HasKey(c => c.RecipeCategoryId);
            builder.Property("Name").HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(25);
        }
    }
}
