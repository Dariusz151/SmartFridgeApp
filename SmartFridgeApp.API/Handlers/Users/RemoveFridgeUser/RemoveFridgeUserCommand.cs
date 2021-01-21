using System;
using MediatR;

namespace SmartFridgeApp.API.Users.RemoveFridgeUser
{
    public class RemoveFridgeUserCommand : IRequest
    {
        public Guid FridgeId { get; }
        public Guid UserId { get; }

        public RemoveFridgeUserCommand(Guid fridgeId, Guid userId)
        {
            FridgeId = fridgeId;
            UserId = userId;
        }
    }
}
