using System;
using System.Collections.Generic;
using MediatR;
            
namespace SmartFridgeApp.API.FridgeItems.GetFridgeItems
{
    public class GetFridgeItemsQuery : IRequest<IEnumerable<FridgeItemDto>>
    {
        public int UserId { get; }
        public int FridgeId { get; }

        public GetFridgeItemsQuery(int userId, int fridgeId)
        {
            UserId = userId;
            FridgeId = fridgeId;
        }
    }
}
