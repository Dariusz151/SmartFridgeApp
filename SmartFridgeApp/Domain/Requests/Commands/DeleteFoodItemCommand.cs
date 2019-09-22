using System;

namespace SmartFridgeApp.Domain.Requests.Commands
{
    public class DeleteFoodItemCommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }

        public DeleteFoodItemCommand(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
