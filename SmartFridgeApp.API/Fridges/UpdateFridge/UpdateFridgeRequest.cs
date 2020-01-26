using System;

namespace SmartFridgeApp.API.Fridges.UpdateFridge
{
    public class UpdateFridgeRequest
    {
        public Guid FridgeId { get; set; }
        public string Name { get; set; }
    }
}
