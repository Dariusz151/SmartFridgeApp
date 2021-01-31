using System;
using MediatR;

namespace SmartFridgeApp.Core.Application.Features
{
    public class AddFridgeItemCommand : IRequest
    {
        public Guid FridgeId { get; }
        public AddFridgeItemDto FridgeItemDto { get; }
        public Guid UserId { get; }

        public AddFridgeItemCommand(Guid fridgeId, AddFridgeItemDto fridgeItemDto, Guid userId)
        {
            FridgeItemDto = fridgeItemDto;
            UserId = userId;
            FridgeId = fridgeId;
        }
    }
}
