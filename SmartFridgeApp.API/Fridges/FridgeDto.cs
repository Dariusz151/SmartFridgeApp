using System;

namespace SmartFridgeApp.API.Fridges
{
    public class FridgeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Desc { get; set; }
    }
}
