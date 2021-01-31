using System;
using MediatR;

namespace SmartFridgeApp.Core.Application.Features
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
