using SmartFridgeApp.Domain.Exceptions;
using SmartFridgeApp.Types;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartFridgeApp.Domain.Models
{
    public class FoodItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; private set; }

        public User User { get; set; }
        public string Name { get; private set; }
        public int Amount { get; private set; }
        public Unit Unit { get; private set; }
        public DateTime EnteredAt { private get; set; }
        public DateTime ExpirationDate { private get; set; }
        public Category Category { get; private set; }
        public bool IsOutdated() => DateTime.Compare(ExpirationDate, DateTime.UtcNow) > 1;

        private FoodItem()
        {

        }

        public FoodItem(string name, int amount, Unit unit)
            : this(name, amount, unit, DateTime.UtcNow.AddYears(1), Category.NotAssigned)
        {

        }

        public FoodItem(string name, int amount, Unit unit, DateTime expirationDate)
            : this(name, amount, unit, expirationDate, Category.NotAssigned)
        {

        }

        public FoodItem(string name, int amount, Unit unit, DateTime expirationDate, Category category)
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
