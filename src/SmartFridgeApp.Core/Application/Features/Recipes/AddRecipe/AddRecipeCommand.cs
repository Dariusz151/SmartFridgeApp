using System.Collections.Generic;
using MediatR;
using SmartFridgeApp.Core.Domain.Entities;

namespace SmartFridgeApp.Core.Application.Features
{
    public class AddRecipeCommand : IRequest<Recipe>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int RecipeCategory { get; set; }
        public int RequiredTime { get; set; }
        public int LevelOfDifficulty { get; set; }

        public List<FoodProductDetailsDto> Products { get; set; }

        public AddRecipeCommand(string name, List<FoodProductDetailsDto> products, string description, int category, int requiredTime, int levelOfDifficulty)
        {
            Name = name;
            Products = products;
            Description = description;
            RecipeCategory = category;
            RequiredTime = requiredTime;
            LevelOfDifficulty = levelOfDifficulty;
        }
    }
}
