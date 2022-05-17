using MediatR;
using SmartFridgeApp.Core.Domain.Shared;
using System;
using System.Collections.Generic;

namespace SmartFridgeApp.Core.Application.Features
{
    public class ConsumeRecipeCommand : IRequest
    {
        public Guid UserId { get; set; }
        public Guid FridgeId { get; set; }
        public List<FoodProductDetails> FoodProducts { get; set; }

        public ConsumeRecipeCommand(Guid userId, Guid fridgeId, List<FoodProductDetails> foodProducts)
        {
            UserId = userId;
            FridgeId = fridgeId;
            FoodProducts = foodProducts;
        }
    }
}
