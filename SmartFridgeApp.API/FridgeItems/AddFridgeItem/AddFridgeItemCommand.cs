using System;
using MediatR;

namespace SmartFridgeApp.API.FridgeItems.AddFridgeItem
{
    public class AddFridgeItemCommand : IRequest
    {
        public Guid FridgeId { get; }
        public AddFridgeItemDto FridgeItemDto { get; }
        public int UserId { get; }

        public AddFridgeItemCommand(Guid fridgeId, AddFridgeItemDto fridgeItemDto, int userId)
        {
            FridgeItemDto = fridgeItemDto;
            UserId = userId;
            FridgeId = fridgeId;
        }
    }
}
