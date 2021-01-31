using MediatR;

namespace SmartFridgeApp.Core.Application.Features
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
