using SmartFridgeApp.Types;
using System;

namespace SmartFridgeApp.Domain.Requests.Commands
{
    public class AddFoodItemCommand : ICommand
    {
        public string Name { get; }
        public Guid UserId { get; }
        public int Amount { get; }
        public Unit Unit { get; }
        public Category Category { get; }
        public DateTime ExpirationDate { get; }

        public AddFoodItemCommand(string name, Guid userId, int amount, Unit unit, Category category, DateTime expirationDate)
        {
            UserId = userId;
            Name = name;
            Amount = amount;
            Unit = unit;
            Category = category;
            ExpirationDate = expirationDate;
        }
    }
}
