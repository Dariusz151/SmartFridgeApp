using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.DomainServices;
using SmartFridgeApp.Domain.Models.Recipes;

namespace SmartFridgeApp.API.Recipes.FindRecipes
{
    public class FindRecipesCommandHandler : IRequestHandler<FindRecipesCommand, IEnumerable<Recipe>>
    {
        private readonly IRecipeFinderService _recipeFinderService;

        public FindRecipesCommandHandler(IRecipeFinderService recipeFinderService)
        {
            _recipeFinderService = recipeFinderService;
        }

        public async Task<IEnumerable<Recipe>> Handle(FindRecipesCommand command, CancellationToken cancellationToken)
        {
            var recipes = await _recipeFinderService.FindMatchingRecipes(command.FoodProducts);

            return recipes.AsEnumerable();
        }
    }
}
