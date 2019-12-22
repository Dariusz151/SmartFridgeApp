using System;
using System.Collections.Generic;
using MediatR;
            
namespace SmartFridgeApp.API.FridgeItems.GetFridgeItems
{
    public class GetFridgeItemsQuery :IRequest<List<FridgeItemDto>>
    {
        public Guid UserId { get; }
        public Guid FridgeId { get; }

        public GetFridgeItemsQuery(Guid userId, Guid fridgeId)
        {
            UserId = userId;
            FridgeId = fridgeId;
        }
    }
}
