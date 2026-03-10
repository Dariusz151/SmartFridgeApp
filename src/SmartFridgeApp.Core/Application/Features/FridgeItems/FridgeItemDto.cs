using System;

namespace SmartFridgeApp.Core.Application.Features
{
    public class FridgeItemDto
    {
        public int FridgeItemId { get; set; }
        public string ProductName { get; set; }
        public int FoodProductId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public float Value { get; set; }
        public string Unit { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserColor { get; set; }
    }
}
