using System;
using MediatR;

namespace SmartFridgeApp.API.Users.AddFridgeUser
{
    public class AddFridgeUserCommand : IRequest
    {
        public Guid FridgeId { get; }
        public UserDto User { get; }

        public AddFridgeUserCommand(Guid fridgeId, UserDto user)
        {
            FridgeId = fridgeId;
            User = user;
        }
    }
}
