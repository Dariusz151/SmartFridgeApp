using SmartFridgeApp.Domain.Models.FoodProducts;
using System;
using System.Collections.Generic;

namespace SmartFridgeApp.API.Handlers.FridgeItems.ConsumeRecipe
{
    public class ConsumeRecipeRequest
    {
        public Guid UserId { get; set; }
        public Guid FridgeId { get; set; }
        public List<FoodProductDetails> FoodProducts { get; set; }
    }
}
