using System;
using MediatR;

namespace SmartFridgeApp.API.Fridges.UpdateFridge
{
    public class UpdateFridgeCommand : IRequest
    {
        public Guid FridgeId { get; set; }
        public string Name { get; set; }

        public UpdateFridgeCommand(Guid fridgeId, string name)
        {
            FridgeId = fridgeId;
            Name = name;    
        }
    }
}
