using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Shared.Domain;

namespace SmartFridgeApp.Core.Application.Events
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
