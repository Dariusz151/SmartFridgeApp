using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFridgeApp.Core.Application.Features
{
    public class RemoveFridgeUserRequest
    {
        public Guid UserId { get; set; }
    }
}
