using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Domain.Entities;

namespace SmartFridgeApp.Core.Contracts.DomainServices
{
    public interface IRecipeFinderService
    {
        Task<List<Recipe>> FindMatchingRecipes(List<short> foodProducts);
    }
}
