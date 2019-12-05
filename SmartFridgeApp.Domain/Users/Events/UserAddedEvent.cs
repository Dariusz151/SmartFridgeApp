using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Users.Events
{
    public class UserAddedEvent : DomainEventBase
    {
        public UserId UserId { get; }

        public UserAddedEvent(UserId userId)
        {
            UserId = UserId;
        }
    }
}
