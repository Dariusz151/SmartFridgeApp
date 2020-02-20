using System;
using System.Collections.Generic;
using MediatR;

namespace SmartFridgeApp.API.Users.GetFridgeUsers
{
    internal class GetFridgeUsersQuery : IRequest<IEnumerable<FridgeUserDto>>
    {
        public int FridgeId { get; }

        public GetFridgeUsersQuery(int fridgeId)
        {
            FridgeId = fridgeId;
        }
    }
}
