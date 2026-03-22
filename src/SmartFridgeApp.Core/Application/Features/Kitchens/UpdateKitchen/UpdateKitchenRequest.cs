using System;

namespace SmartFridgeApp.Core.Application.Features
{
    public class UpdateKitchenRequest
    {
        public Guid kitchenId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
    }
}
