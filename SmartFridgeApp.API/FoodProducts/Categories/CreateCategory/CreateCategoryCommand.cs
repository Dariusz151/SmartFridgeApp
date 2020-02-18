using MediatR;

namespace SmartFridgeApp.API.FoodProducts.Categories.CreateCategory
{
    public class CreateCategoryCommand : IRequest
    {
        public string Name { get; }

        public CreateCategoryCommand(string name)
        {
            Name = name;
        }
    }
}
