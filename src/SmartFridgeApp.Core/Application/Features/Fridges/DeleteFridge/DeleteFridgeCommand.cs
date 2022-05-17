using System;
using MediatR;

namespace SmartFridgeApp.Core.Application.Features
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
