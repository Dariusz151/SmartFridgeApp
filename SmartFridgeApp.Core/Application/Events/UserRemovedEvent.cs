using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.SeedWork;

namespace SmartFridgeApp.Core.Application.Events
{
    public class UserRemovedEvent : DomainEventBase
    {
        public User User { get; }

        public UserRemovedEvent(User user)
        {
            User = user;
        }
    }
}
