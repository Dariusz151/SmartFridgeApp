using System;
using MediatR;

namespace SmartFridgeApp.Core.Application.Features
{
    public class UpdateRecipeCommand : IRequest
    {
        public Guid RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RequiredTime { get; set; }
        public int LevelOfDifficulty { get; set; }
        public int RecipeCategory { get; set; }

        public UpdateRecipeCommand(Guid recipeId, string name, string description, int category, int requiredTime, int levelOfDifficulty)
        {
            Name = name;
            Description = description;
            RecipeCategory = category;
            RecipeId = recipeId;
            RequiredTime = requiredTime;
            LevelOfDifficulty = levelOfDifficulty;
        }
    }
}
