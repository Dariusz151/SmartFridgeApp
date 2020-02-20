using System;
using MediatR;

namespace SmartFridgeApp.API.Users.RemoveFridgeUser
{
    public class RemoveFridgeUserCommand : IRequest
    {
        public int FridgeId { get; }
        public int UserId { get; }

        public RemoveFridgeUserCommand(int fridgeId, int userId)
        {
            FridgeId = fridgeId;
            UserId = userId;
        }
    }
}
