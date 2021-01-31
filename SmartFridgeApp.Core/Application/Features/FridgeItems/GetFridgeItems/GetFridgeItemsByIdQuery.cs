using MediatR;
using System;
using System.Collections.Generic;

namespace SmartFridgeApp.Core.Application.Features
{
    public class GetFridgeItemsByIdQuery : IRequest<IEnumerable<FridgeItemDto>>
    {
        public Guid FridgeId { get; }

        public GetFridgeItemsByIdQuery(Guid fridgeId)
        {
            FridgeId = fridgeId;
        }
    }
}
