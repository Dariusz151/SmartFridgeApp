using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SmartFridgeApp.API.Users.GetFridgeUsers
{
    internal class GetFridgeUsersQueryHandler : IRequestHandler<GetFridgeUsersQuery, List<UserDto>>
    {
        
        public Task<List<UserDto>> Handle(GetFridgeUsersQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
