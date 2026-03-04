using System;
using Newtonsoft.Json;
using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.Infrastructure.Notifications;

public class RecipeAddedNotification : DomainNotificationBase<RecipeAddedEvent>
{
    public Guid RecipeId { get; }

    public RecipeAddedNotification(RecipeAddedEvent domainEvent) : base(domainEvent)
    {
        RecipeId = domainEvent.Recipe.RecipeId;
    }

    [JsonConstructor]
    public RecipeAddedNotification(Guid recipeId) : base(null)
    {
        RecipeId = recipeId;
    }
}
