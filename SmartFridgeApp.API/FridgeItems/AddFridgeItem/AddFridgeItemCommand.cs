using System;
using MediatR;

namespace SmartFridgeApp.API.FridgeItems.AddFridgeItem
{
    public class AddFridgeItemCommand : IRequest
    {
        public Guid FridgeId { get; }
        public FridgeItemDto FridgeItemDto { get; }
        public Guid UserId { get; }

        public AddFridgeItemCommand(Guid fridgeId, FridgeItemDto fridgeItemDto, Guid userId)
        {
            FridgeItemDto = fridgeItemDto;
            UserId = userId;
            FridgeId = fridgeId;
        }
    }
}
