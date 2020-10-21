using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.Recipes;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.SeedWork.Exceptions;
using SmartFridgeApp.Domain.Shared;
using SmartFridgeApp.Infrastructure.Extensions;

namespace SmartFridgeApp.API.Recipes.AddRecipe
{
    public class AddRecipeCommandHandler : IRequestHandler<AddRecipeCommand, Recipe>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IFoodProductRepository _foodProductRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddRecipeCommandHandler(
            IRecipeRepository recipeRepository, IUnitOfWork unitOfWork, IFoodProductRepository foodProductRepository)
        {
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
            _foodProductRepository = foodProductRepository;
        }

        public async Task<Recipe> Handle(AddRecipeCommand command, CancellationToken cancellationToken)
        {
            if (!await ValidateInsertedIndexes(command))
            {
                throw new DomainException("DomainException", "Some product id's does not exist in database.");
            }

            var recipeCategory = await _recipeRepository.GetRecipeCategoryByIdAsync(command.RecipeCategory);

            var recipe = new Recipe(
                command.Name,
                command.Description,
                recipeCategory,
                command.Products);

            await _recipeRepository.AddRecipeAsync(recipe);
            await _unitOfWork.CommitAsync(cancellationToken);

            return recipe;
        }

        private async Task<bool> ValidateInsertedIndexes(AddRecipeCommand command)
        {
            var allFoodProducts = await _foodProductRepository.GetAllAsync();
            List<short> allFoodProductsId = allFoodProducts.Select(x => x.FoodProductId).ToList();
            List<short> insertedFoodProductsId = command.Products.Select(x => x.FoodProductId).ToList();

            return allFoodProductsId.ContainsAllItems(insertedFoodProductsId);
        }
    }
}
