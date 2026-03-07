using System;
using Newtonsoft.Json;
using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.Infrastructure.Notifications;

public class UserAddedNotification : DomainNotificationBase<UserAddedEvent>
{
    public Guid UserId { get; }
    public string UserName { get; }

    public UserAddedNotification(UserAddedEvent domainEvent) : base(domainEvent)
    {
        UserId = domainEvent.User.Id;
        UserName = domainEvent.User.Name;
    }

    [JsonConstructor]
    public UserAddedNotification(Guid userId, string userName) : base(null)
    {
        UserId = userId;
        UserName = userName;
    }
}
