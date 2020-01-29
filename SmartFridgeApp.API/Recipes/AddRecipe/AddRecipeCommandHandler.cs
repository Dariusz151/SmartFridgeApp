using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.API.Fridges;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.Recipes;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Shared;

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
            var allFoodProducts = await _foodProductRepository.GetAllAsync();

            //TODO: 'Sugar syntax' to this. LINQ expression.

            List<FoodProduct> currentFoodProducts = (from foodProduct in allFoodProducts from el in command.ProductIds where foodProduct.FoodProductId.Equals(el) select foodProduct).ToList();
            
            
            var recipe = new Recipe(
                command.Name, 
                command.Description,
                command.DifficultyLevel,
                command.MinutesRequired,
                command.Category,
                currentFoodProducts);

            await _recipeRepository.AddRecipeAsync(recipe);
            await _unitOfWork.CommitAsync(cancellationToken);

            return recipe;
        }
    }
}
