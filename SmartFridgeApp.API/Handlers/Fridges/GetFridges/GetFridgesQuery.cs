using System.Collections.Generic;
using MediatR;

namespace SmartFridgeApp.API.Fridges.GetFridges
{
    public class GetFridgesQuery :IRequest<IEnumerable<FridgeDto>>
    {
        public GetFridgesQuery()
        {
                
        }
    }
}
