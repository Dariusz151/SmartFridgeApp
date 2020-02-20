using System;

namespace SmartFridgeApp.API.Fridges.UpdateFridge
{
    public class UpdateFridgeRequest
    {
        public int FridgeId { get; set; }
        public string Name { get; set; }
    }
}
