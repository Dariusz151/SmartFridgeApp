using System;

namespace SmartFridgeApp.Core.Application.Features
{
    public class KitchenDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Desc { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
