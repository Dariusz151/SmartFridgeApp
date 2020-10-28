﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.Models.Recipes;
using SmartFridgeApp.Domain.SeedWork;
using Unit = MediatR.Unit;

namespace SmartFridgeApp.API.Recipes.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRecipeCommandHandler(
            IRecipeRepository recipeRepository, IUnitOfWork unitOfWork)
        {
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateRecipeCommand command, CancellationToken cancellationToken)
        {
            // TODO: validate if recipe exists

            var recipe = await _recipeRepository.GetRecipeByIdAsync(command.RecipeId);
            var newRecipeCategory = await _recipeRepository.GetRecipeCategoryByIdAsync(command.RecipeCategory);

            recipe.UpdateRecipe(command.Name, command.Description, newRecipeCategory, command.RequiredTime, command.LevelOfDifficulty);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
