using System;
using MediatR;

namespace SmartFridgeApp.API.Users.UpdateFridgeUser
{
    public class UpdateFridgeUserCommand : IRequest
    {
        public int FridgeId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public UpdateFridgeUserCommand(int userId, string name, int fridgeId)
        {
            FridgeId = fridgeId;
            UserId = userId;
            Name = name;
        }
    }
}
