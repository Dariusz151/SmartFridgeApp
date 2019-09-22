using SmartFridgeApp.Types;
using System;

namespace SmartFridgeApp.Domain.Requests
{
    public class AddFoodItemRequest
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public Unit Unit { get; set; }
        public Category Category { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
