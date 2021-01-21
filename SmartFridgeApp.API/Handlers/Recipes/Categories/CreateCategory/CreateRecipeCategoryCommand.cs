using MediatR;

namespace SmartFridgeApp.API.Recipes.Categories.CreateCategory
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
