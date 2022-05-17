using System;

namespace SmartFridgeApp.Core.Application.Features
{
    public class UpdateFridgeRequest
    {
        public Guid FridgeId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
    }
}
