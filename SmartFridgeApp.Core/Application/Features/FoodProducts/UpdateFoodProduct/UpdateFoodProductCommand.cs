using MediatR;

namespace SmartFridgeApp.Core.Application.Features
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
