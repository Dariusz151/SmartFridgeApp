using SmartFridgeApp.Domain.Exceptions;
using SmartFridgeApp.Types;
using System;

namespace SmartFridgeApp.Domain.Models
{
    public class FoodItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public Unit Unit { get; set; }
        public DateTime EnteredAt { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Category Category { get; set; }
        public bool IsOutdated() => DateTime.Compare(ExpirationDate, DateTime.UtcNow) > 1;

        private FoodItem()
        {

        }

        public FoodItem(string name, int amount, Unit unit)
        {
            CreateFoodItem(name, amount, unit, DateTime.UtcNow.AddYears(1), Category.NotAssigned);
        }
        
        public FoodItem(string name, int amount, Unit unit, DateTime expirationDate)
        {
            CreateFoodItem(name, amount, unit, expirationDate, Category.NotAssigned);
        }

        public FoodItem(string name, int amount, Unit unit, DateTime expirationDate, Category category)
        {
            CreateFoodItem(name, amount, unit, expirationDate, category);
        }

        private void CreateFoodItem (string name, int amount, Unit unit, DateTime expirationDate, Category category)
        {
            if (DateTime.Compare(DateTime.UtcNow.Date, expirationDate.Date) >= 0)
                throw new InvalidFoodItemException($"This food product is expired!");
            if (unit.Equals(Unit.NotAssigned))
                throw new InvalidFoodItemException($"Food item must have any unit of measure.");
            Id = new Guid();
            Name = name;
            Amount = amount;
            Unit = unit;
            EnteredAt = DateTime.UtcNow;
            ExpirationDate = expirationDate;
            Category = Category;
        }
    }
}
