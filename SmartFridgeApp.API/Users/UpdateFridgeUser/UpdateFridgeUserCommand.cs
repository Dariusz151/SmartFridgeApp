using System;
using MediatR;

namespace SmartFridgeApp.API.Users.UpdateFridgeUser
{
    public class UpdateFridgeUserCommand : IRequest
    {
        public Guid FridgeId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public UpdateFridgeUserCommand(int userId, string name, Guid fridgeId)
        {
            FridgeId = fridgeId;
            UserId = userId;
            Name = name;
        }
    }
}
