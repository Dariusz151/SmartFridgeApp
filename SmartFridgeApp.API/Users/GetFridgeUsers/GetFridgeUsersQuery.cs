using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace SmartFridgeApp.API.Users.GetFridgeUsers
{
    internal class GetFridgeUsersQuery : IRequest<List<UserDto>>
    {
        public Guid FridgeId { get; }

        public GetFridgeUsersQuery(Guid fridgeId)
        {
            FridgeId = fridgeId;
        }
    }
}
