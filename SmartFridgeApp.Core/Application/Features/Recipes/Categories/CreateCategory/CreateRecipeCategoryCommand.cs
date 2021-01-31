using MediatR;

namespace SmartFridgeApp.Core.Application.Features
{
    public class CreateRecipeCategoryCommand : IRequest
    {
        public string Name { get; }

        public CreateRecipeCategoryCommand(string name)
        {
            Name = name;
        }
    }
}
