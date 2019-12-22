using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace SmartFridgeApp.API.FridgeItems.RemoveFridgeItem
{
    public class RemoveFridgeItemCommand : IRequest
    {
        public Guid FridgeItemId { get; set; }
        public Guid FridgeId { get; set; }
        public Guid UserId { get; set; }

        public RemoveFridgeItemCommand(Guid fridgeItemId, Guid userId, Guid fridgeId)
        {
            FridgeItemId = fridgeItemId;
            UserId = userId;
            FridgeId = fridgeId;
        }
    }
}
