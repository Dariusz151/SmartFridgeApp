using MediatR;

namespace SmartFridgeApp.API.Users.AddFridgeUser
{
    public class AddFridgeUserCommand : IRequest
    {
        public int FridgeId { get; }
        public UserDto User { get; }

        public AddFridgeUserCommand(int fridgeId, UserDto user)
        {
            FridgeId = fridgeId;
            User = user;
        }
    }
}
