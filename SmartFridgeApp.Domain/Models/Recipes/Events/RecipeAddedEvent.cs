using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Models.Recipes.Events
{
    public class RecipeAddedEvent : DomainEventBase
    {
        public Recipe Recipe { get; }
        public RecipeAddedEvent(Recipe recipe)
        {
            Recipe = recipe;
        }
    }
}
