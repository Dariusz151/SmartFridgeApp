using System;
using MediatR;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.FridgeItems.ConsumeFridgeItem
{
    public class ConsumeFridgeItemCommand : IRequest
    {
        public Guid FridgeItemId { get; set; }
        public Guid FridgeId { get; set; }
        public Guid UserId { get; set; }
        public AmountValue AmountValue { get; set; }

        public ConsumeFridgeItemCommand(Guid fridgeItemId, Guid userId, Guid fridgeId, AmountValue amountValue)
        {
            FridgeItemId = fridgeItemId;
            UserId = userId;
            FridgeId = fridgeId;
            AmountValue = amountValue;
        }
    }
}
