using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Shared.Domain;

namespace SmartFridgeApp.Core.Application.Events
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
