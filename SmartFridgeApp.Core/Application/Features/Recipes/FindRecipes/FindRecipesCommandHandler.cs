using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Core.Contracts.DomainServices;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.SeedWork;

namespace SmartFridgeApp.Core.Application.Features
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
