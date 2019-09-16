using SmartFridgeApp.Domain.Exceptions;
using SmartFridgeApp.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public FoodItem(string name, int amount, Unit unit, DateTime expirationDate, Category category)
        {
            if (DateTime.Compare(DateTime.UtcNow,expirationDate) > 0)
                throw new InvalidFoodItemException($"This food product is expired!");
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
