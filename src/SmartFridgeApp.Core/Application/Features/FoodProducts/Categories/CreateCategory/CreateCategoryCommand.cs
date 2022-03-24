using MediatR;

namespace SmartFridgeApp.Core.Application.Features
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
