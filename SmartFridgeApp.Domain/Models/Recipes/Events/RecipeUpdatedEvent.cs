using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Models.Recipes.Events
{
    public class RecipeUpdatedEvent : DomainEventBase
    {
        public Recipe Recipe { get; }
        public RecipeUpdatedEvent(Recipe recipe)
        {
            Recipe = recipe;
        }
    }
}
