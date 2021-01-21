using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.Recipes.AddRecipe
{
    public class FoodProductDetailsDto
    {
        public short FoodProductId { get; set; }
        public bool IsOptional { get; set; }
        public AmountValue AmountValue { get; set; }
    }
}
