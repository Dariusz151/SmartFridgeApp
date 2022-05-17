using System;
using MediatR;

namespace SmartFridgeApp.Core.Application.Features
{
    public class RemoveFridgeItemCommand : IRequest
    {
        public long FridgeItemId { get; set; }
        public Guid FridgeId { get; set; }
        public Guid UserId { get; set; }

        public RemoveFridgeItemCommand(long fridgeItemId, Guid userId, Guid fridgeId)
        {
            FridgeItemId = fridgeItemId;
            UserId = userId;
            FridgeId = fridgeId;
        }
    }
}
