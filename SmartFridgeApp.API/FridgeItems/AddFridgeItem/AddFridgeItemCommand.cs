using System;
using MediatR;

namespace SmartFridgeApp.API.FridgeItems.AddFridgeItem
{
    public class AddFridgeItemCommand : IRequest
    {
        public int FridgeId { get; }
        public AddFridgeItemDto FridgeItemDto { get; }
        public int UserId { get; }

        public AddFridgeItemCommand(int fridgeId, AddFridgeItemDto fridgeItemDto, int userId)
        {
            FridgeItemDto = fridgeItemDto;
            UserId = userId;
            FridgeId = fridgeId;
        }
    }
}
