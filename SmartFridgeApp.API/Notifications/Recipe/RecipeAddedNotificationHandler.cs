﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.DomainServices;

namespace SmartFridgeApp.API.Notifications.Recipe
{
    public class RecipeAddedNotificationHandler : INotificationHandler<RecipeAddedNotification>
    {
        private readonly INotifier _notifier;

        public RecipeAddedNotificationHandler(INotifier notifier)
        {
            _notifier = notifier;
        }

        public async Task Handle(RecipeAddedNotification notification, CancellationToken cancellationToken)
        {
            var recipeId = notification.RecipeId.ToString();
            _notifier.SendMessage(recipeId, "Recipe added!");
        }
    }
}
