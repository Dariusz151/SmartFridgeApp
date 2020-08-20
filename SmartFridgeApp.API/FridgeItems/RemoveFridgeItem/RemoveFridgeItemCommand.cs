﻿using System;
using MediatR;

namespace SmartFridgeApp.API.FridgeItems.RemoveFridgeItem
{
    public class RemoveFridgeItemCommand : IRequest
    {
        public long FridgeItemId { get; set; }
        public Guid FridgeId { get; set; }
        public Guid UserId { get; set; }

        public RemoveFridgeItemCommand(long fridgeItemId, Guid userId, Guid fridgeId)
        {
            FridgeItemId = fridgeItemId;
            UserId = userId;
            FridgeId = fridgeId;
        }
    }
}
