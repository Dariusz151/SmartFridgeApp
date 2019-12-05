using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Users.Events
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
