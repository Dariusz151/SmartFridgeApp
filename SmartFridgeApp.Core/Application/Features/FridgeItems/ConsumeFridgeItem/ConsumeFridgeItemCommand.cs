using System;
using MediatR;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Application.Features
{
    public class ConsumeFridgeItemCommand : IRequest
    {
        public long FridgeItemId { get; set; }
        public Guid FridgeId { get; set; }
        public Guid UserId { get; set; }
        public AmountValue AmountValue { get; set; }

        public ConsumeFridgeItemCommand(long fridgeItemId, Guid userId, Guid fridgeId, AmountValue amountValue)
        {
            FridgeItemId = fridgeItemId;
            UserId = userId;
            FridgeId = fridgeId;
            AmountValue = amountValue;
        }
    }
}
