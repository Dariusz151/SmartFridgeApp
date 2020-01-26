using MediatR;

namespace SmartFridgeApp.API.FoodProducts.DeleteFoodProduct
{
    public class DeleteFoodProductCommand : IRequest
    {
        public int FoodProductId { get; }

        public DeleteFoodProductCommand(int foodProductId)
        {
            FoodProductId = foodProductId;
        }
    }
}
