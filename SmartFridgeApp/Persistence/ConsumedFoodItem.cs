using SmartFridgeApp.Types;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartFridgeApp.Persistence
{
    public class ConsumedFoodItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        
        public string FoodItemName { get; private set; }
        public int Amount { get; private set; }
        public Unit Unit { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime ConsumedAt { get; private set; }
       
        private ConsumedFoodItem()
        {

        }

        public ConsumedFoodItem(string name, Guid userId, int amount, Unit unit)
        {
            FoodItemName = name;
            Amount = amount;
            Unit = unit;
            UserId = userId;
            ConsumedAt = DateTime.UtcNow;
        }
    }
}
