using System;

namespace SmartFridgeApp.Core.Application.Features
{
    public class UpdateFridgeUserRequest
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
    }
}
