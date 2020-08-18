using System;
using MediatR;

namespace SmartFridgeApp.API.Users.RemoveFridgeUser
{
    public class RemoveFridgeUserCommand : IRequest
    {
        public Guid FridgeId { get; }
        public int UserId { get; }

        public RemoveFridgeUserCommand(Guid fridgeId, int userId)
        {
            FridgeId = fridgeId;
            UserId = userId;
        }
    }
}
