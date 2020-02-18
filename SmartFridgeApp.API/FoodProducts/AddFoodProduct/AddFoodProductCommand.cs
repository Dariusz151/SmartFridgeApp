using MediatR;

namespace SmartFridgeApp.API.FoodProducts.AddFoodProduct
{
    public class AddFoodProductCommand : IRequest
    {
        public string Name { get; }
        public string Category { get; }

        public AddFoodProductCommand(string name, string category)
        {
            Name = name;
            Category = category;
        }
    }
}
