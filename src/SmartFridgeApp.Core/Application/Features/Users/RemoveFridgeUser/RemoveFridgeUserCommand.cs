using System;
using MediatR;

namespace SmartFridgeApp.Core.Application.Features
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
