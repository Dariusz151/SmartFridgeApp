using System;
using SmartFridgeApp.Domain.Models.Users;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Models.Fridges.Events
{
    public class UserAddedEvent : DomainEventBase
    {
        public User User { get; }

        public UserAddedEvent(User user)
        {
            User = user;
        }
    }
}
