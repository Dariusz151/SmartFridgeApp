using System;
using MediatR;

namespace SmartFridgeApp.API.Fridges
{
    public class MarkFridgeAsWelcomedCommand : IRequest
    {
        public MarkFridgeAsWelcomedCommand(Guid fridgeId)
        {
            FridgeId = fridgeId;
        }

        public Guid FridgeId { get; }
    }
}
