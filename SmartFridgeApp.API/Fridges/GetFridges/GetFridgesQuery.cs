using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace SmartFridgeApp.API.Fridges.GetFridges
{
    public class GetFridgesQuery :IRequest<List<FridgeDto>>
    {
        public GetFridgesQuery()
        {
                
        }
    }
}
