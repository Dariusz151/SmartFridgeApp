using MediatR;

namespace SmartFridgeApp.Core.Application.Features
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
