using MediatR;
using SmartFridgeApp.Domain.Models.FoodProducts;

namespace SmartFridgeApp.API.FoodProducts.AddFoodProduct
{
    public class AddFoodProductCommand : IRequest
    {
        public string Name { get; }
        public int CategoryId { get; }

        public AddFoodProductCommand(string name, int categoryId)
        {
            Name = name;
            CategoryId = categoryId;
        }
    }
}
