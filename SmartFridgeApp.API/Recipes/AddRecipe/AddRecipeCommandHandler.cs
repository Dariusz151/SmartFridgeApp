using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.API.Fridges;
using SmartFridgeApp.Domain.Fridges;
using SmartFridgeApp.Domain.Recipes;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.API.Recipes.AddRecipe
{
    public class AddRecipeCommandHandler : IRequestHandler<AddRecipeCommand, Recipe>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddRecipeCommandHandler(
            IRecipeRepository recipeRepository, IUnitOfWork unitOfWork)
        {
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Recipe> Handle(AddRecipeCommand command, CancellationToken cancellationToken)
        {
            var recipe = new Recipe();

            await _recipeRepository.AddRecipeAsync(recipe);

            await _unitOfWork.CommitAsync(cancellationToken);

            return recipe;
        }
    }
}
