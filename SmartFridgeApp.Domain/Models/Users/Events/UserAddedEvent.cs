using System;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Models.Users.Events
{
    public class UserAddedEvent : DomainEventBase
    {
        public Guid UserId { get; }

        public UserAddedEvent(Guid userId)
        {
            UserId = UserId;
        }
    }
}
