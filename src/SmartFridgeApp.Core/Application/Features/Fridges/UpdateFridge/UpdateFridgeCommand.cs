using System;
using MediatR;

namespace SmartFridgeApp.Core.Application.Features
{
    public class UpdateFridgeCommand : IRequest
    {
        public Guid FridgeId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }

        public UpdateFridgeCommand(Guid fridgeId, string name, string desc)
        {
            FridgeId = fridgeId;
            Name = name;
            Desc = desc;
        }
    }
}
