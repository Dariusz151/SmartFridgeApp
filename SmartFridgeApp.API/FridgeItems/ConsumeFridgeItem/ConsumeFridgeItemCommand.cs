using System;
using MediatR;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.FridgeItems.ConsumeFridgeItem
{
    public class ConsumeFridgeItemCommand : IRequest
    {
        public long FridgeItemId { get; set; }
        public int FridgeId { get; set; }
        public int UserId { get; set; }
        public AmountValue AmountValue { get; set; }

        public ConsumeFridgeItemCommand(long fridgeItemId, int userId, int fridgeId, AmountValue amountValue)
        {
            FridgeItemId = fridgeItemId;
            UserId = userId;
            FridgeId = fridgeId;
            AmountValue = amountValue;
        }
    }
}
