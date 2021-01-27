using MediatR;
using SmartFridgeApp.Domain.Models.FoodProducts;
using System;
using System.Collections.Generic;

namespace SmartFridgeApp.API.Handlers.FridgeItems.ConsumeRecipe
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
