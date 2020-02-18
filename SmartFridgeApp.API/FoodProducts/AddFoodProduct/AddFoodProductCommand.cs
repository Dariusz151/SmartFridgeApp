using MediatR;
using SmartFridgeApp.Domain.Models.FoodProducts;

namespace SmartFridgeApp.API.FoodProducts.AddFoodProduct
{
    public class AddFoodProductCommand : IRequest
    {
        public string Name { get; }
        public Category Category { get; }

        public AddFoodProductCommand(string name, Category category)
        {
            Name = name;
            Category = category;
        }
    }
}
