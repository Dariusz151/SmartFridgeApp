using SmartFridgeApp.Domain.Models.Users;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Models.Fridges.Events
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
