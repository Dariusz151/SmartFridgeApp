using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Models.Users.Events
{
    public class UserUpdatedEvent : DomainEventBase
    {
        public User User { get; set; }

        public UserUpdatedEvent(User user)
        {
            User = user;    
        }
    }
}
