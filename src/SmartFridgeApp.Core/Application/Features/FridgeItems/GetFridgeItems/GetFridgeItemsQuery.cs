using System;
using System.Collections.Generic;
using MediatR;

namespace SmartFridgeApp.Core.Application.Features
{
    public class GetFridgeItemsQuery : IRequest<IEnumerable<FridgeItemDto>>
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
