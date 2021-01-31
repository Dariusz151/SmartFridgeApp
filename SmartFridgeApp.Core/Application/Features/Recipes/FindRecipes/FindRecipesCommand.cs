using System.Collections.Generic;
using MediatR;
using SmartFridgeApp.Core.Domain.Entities;

namespace SmartFridgeApp.Core.Application.Features
{
    public class FindRecipesCommand : IRequest<IEnumerable<Recipe>>
    {
        public List<short> FoodProducts { get; set; }

        public FindRecipesCommand(List<short> foodProducts)
        {
            this.FoodProducts = foodProducts;
        }
    }
}
