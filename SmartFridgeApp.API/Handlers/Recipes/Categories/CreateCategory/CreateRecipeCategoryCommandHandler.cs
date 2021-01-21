using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.Recipes;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.API.Recipes.Categories.CreateCategory
{
    public class CreateRecipeCategoryCommandHandler : IRequestHandler<CreateRecipeCategoryCommand>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRecipeCategoryCommandHandler(IRecipeRepository recipeRepository, IUnitOfWork unitOfWork)
        {
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateRecipeCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = new RecipeCategory(command.Name);

            await _recipeRepository.CreateRecipeCategoryAsync(category);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
