using System;
using System.Collections.Generic;
using MediatR;

namespace SmartFridgeApp.API.Users.GetFridgeUsers
{
    internal class GetFridgeUsersQuery : IRequest<IEnumerable<FridgeUserDto>>
    {
        public Guid FridgeId { get; }

        public GetFridgeUsersQuery(Guid fridgeId)
        {
            FridgeId = fridgeId;
        }
    }
}
