using MediatR;

namespace SmartFridgeApp.API.FoodProducts.AddFoodProduct
{
    public class AddFoodProductCommand : IRequest
    {
        public string Name { get; }

        public AddFoodProductCommand(string name)
        {
            Name = name;
        }
    }
}
