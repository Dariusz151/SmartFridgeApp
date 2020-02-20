using System;
using MediatR;

namespace SmartFridgeApp.API.Fridges.UpdateFridge
{
    public class UpdateFridgeCommand : IRequest
    {
        public int FridgeId { get; set; }
        public string Name { get; set; }

        public UpdateFridgeCommand(int fridgeId, string name)
        {
            FridgeId = fridgeId;
            Name = name;    
        }
    }
}
