using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.API.Fridges;
using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Domain.Fridges;
using SmartFridgeApp.Domain.RecipeFoodProducts;
using SmartFridgeApp.Domain.Recipes;
using SmartFridgeApp.Domain.SeedWork;

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

            //TODO: 'Sugar syntax' to this LINQ expression.

            ICollection<FoodProduct> currentFoodProducts = (from foodProduct in allFoodProducts from el in command.ProductIds where foodProduct.FoodProductId.Equals(el) select foodProduct).ToList();

            var recipeFoodProducts = new List<RecipeFoodProduct>();

            foreach (var el in currentFoodProducts)
            {
                var rfp = new RecipeFoodProduct();
                rfp.FoodProduct = el;
                recipeFoodProducts.Add(rfp);
            }
            
            var recipe = new Recipe(command.Name, recipeFoodProducts);

            await _recipeRepository.AddRecipeAsync(recipe);
            await _unitOfWork.CommitAsync(cancellationToken);

            return recipe;
        }
    }
}
