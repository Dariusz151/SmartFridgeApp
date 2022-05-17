using System.Collections.Generic;
using MediatR;

namespace SmartFridgeApp.Core.Application.Features
{
    public class GetFridgesQuery : IRequest<IEnumerable<FridgeDto>>
    {
        public GetFridgesQuery()
        {
                
        }
    }
}
