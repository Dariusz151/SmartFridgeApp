using System;
using MediatR;

namespace SmartFridgeApp.API.Fridges.DeleteFridge
{
    public class DeleteFridgeCommand : IRequest
    {
        public Guid FridgeId { get; }

        public DeleteFridgeCommand(Guid fridgeId)
        {
            FridgeId = fridgeId;
        }
    }
}
