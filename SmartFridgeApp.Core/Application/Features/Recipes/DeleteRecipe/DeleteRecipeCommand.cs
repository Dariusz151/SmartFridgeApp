using System;
using MediatR;

namespace SmartFridgeApp.Core.Application.Features
{
    public class DeleteRecipeCommand : IRequest
    {
        public Guid RecipeId { get; set; }

        public DeleteRecipeCommand(Guid recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
