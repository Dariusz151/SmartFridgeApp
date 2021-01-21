using System;
using MediatR;

namespace SmartFridgeApp.API.Users.UpdateFridgeUser
{
    public class UpdateFridgeUserCommand : IRequest
    {
        public Guid FridgeId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }

        public UpdateFridgeUserCommand(Guid userId, string name, Guid fridgeId)
        {
            FridgeId = fridgeId;
            UserId = userId;
            Name = name;
        }
    }
}
