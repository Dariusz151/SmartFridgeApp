using System;
using MediatR;

namespace SmartFridgeApp.API.FridgeItems.RemoveFridgeItem
{
    public class RemoveFridgeItemCommand : IRequest
    {
        public long FridgeItemId { get; set; }
        public int FridgeId { get; set; }
        public int UserId { get; set; }

        public RemoveFridgeItemCommand(long fridgeItemId, int userId, int fridgeId)
        {
            FridgeItemId = fridgeItemId;
            UserId = userId;
            FridgeId = fridgeId;
        }
    }
}
