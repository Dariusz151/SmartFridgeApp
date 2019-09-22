using System;

namespace SmartFridgeApp.Domain.Requests.Commands
{
    public class ConsumeFoodItemCommand : ICommand
    {
        public Guid FoodItemId { get; }
        public Guid UserId { get; }

        public ConsumeFoodItemCommand(Guid foodItemId, Guid userId)
        {
            FoodItemId = foodItemId;
            UserId = userId;
        }
    }
}
