
using SmartFridgeApp.Core.Domain.Shared;
using System;
using System.Collections.Generic;

namespace SmartFridgeApp.Core.Application.Features
{
    public class ConsumeRecipeRequest
    {
        public Guid UserId { get; set; }
        public Guid FridgeId { get; set; }
        public List<FoodProductDetails> FoodProducts { get; set; }
    }
}
