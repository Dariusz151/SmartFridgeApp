using System;
using Newtonsoft.Json;
using SmartFridgeApp.Domain.Models.Fridges.Events;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.API.Users.Notifications
{
    public class UserAddedNotification : DomainNotificationBase<UserAddedEvent>
    {
        public Guid UserId { get; }
        public string UserName { get; }

        public UserAddedNotification(UserAddedEvent domainEvent) : base(domainEvent)
        {
            this.UserId = domainEvent.User.Id;
            this.UserName = domainEvent.User.Name;
        }

        [JsonConstructor]
        public UserAddedNotification(Guid userId, string userName) : base(null)
        {
            this.UserId = userId;
            this.UserName = userName;
        }
    }
}
