using System;
using System.Collections.Generic;
using MediatR;

namespace SmartFridgeApp.Core.Application.Features
{
    public class GetFridgeUsersQuery : IRequest<IEnumerable<FridgeUserDto>>
    {
        public Guid FridgeId { get; }

        public GetFridgeUsersQuery(Guid fridgeId)
        {
            FridgeId = fridgeId;
        }
    }
}
