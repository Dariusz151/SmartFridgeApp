using System;
using System.Collections.Generic;
using MediatR;
            
namespace SmartFridgeApp.API.FridgeItems.GetFridgeItems
{
    public class GetFridgeItemsQuery : IRequest<IEnumerable<FridgeItemDto>>
    {
        public int UserId { get; }
        public Guid FridgeId { get; }

        public GetFridgeItemsQuery(int userId, Guid fridgeId)
        {
            UserId = userId;
            FridgeId = fridgeId;
        }
    }
}
