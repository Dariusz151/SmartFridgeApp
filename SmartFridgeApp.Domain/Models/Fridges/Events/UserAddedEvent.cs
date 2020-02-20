using System;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Models.Fridges.Events
{
    public class UserAddedEvent : DomainEventBase
    {
        public int UserId { get; }

        public UserAddedEvent(int userId)
        {
            UserId = UserId;
        }
    }
}
