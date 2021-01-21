using MediatR;

namespace SmartFridgeApp.API.FoodProducts.UpdateFoodProduct
{
    public class UpdateFoodProductCommand : IRequest
    {
        public int FoodProductId { get; }
        public string FoodProductName { get; }

        public UpdateFoodProductCommand(int foodProductId, string foodProductName)
        {
            FoodProductId = foodProductId;
            FoodProductName = foodProductName;
        }
    }
}
