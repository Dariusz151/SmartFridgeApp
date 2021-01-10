﻿
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.Domain.Models.FoodProducts
{
    public class FoodProductDetails
    {
        public short FoodProductId { get; set; }
        public string FoodProductName { get; set; }
        public AmountValue AmountValue { get; set; }
        public bool IsOptional { get; set; }

        private FoodProductDetails()
        {
            
        }

        public void SetOptional()
        {
            IsOptional = true;
        }

        public FoodProductDetails(short productId, AmountValue amountValue) : this(productId, "", amountValue, false)
        {
        }

        public FoodProductDetails(short productId, string foodProductName, AmountValue amountValue) : this(productId, foodProductName, amountValue, false)
        {
        }

        public FoodProductDetails(short productId, string foodProductName, AmountValue amountValue, bool isOptional)
        {
            FoodProductName = foodProductName;
            FoodProductId = productId;
            AmountValue = amountValue;
            IsOptional = isOptional;
        }
    }
}
