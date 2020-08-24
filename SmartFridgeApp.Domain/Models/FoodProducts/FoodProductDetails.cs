
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.Domain.Models.FoodProducts
{
    public class FoodProductDetails
    {
        public short FoodProductId { get; set; }
        public AmountValue AmountValue { get; set; }

        private FoodProductDetails()
        {
            
        }

        public FoodProductDetails(short productId, AmountValue amountValue)
        {
            FoodProductId = productId;
            AmountValue = amountValue;
        }
    }
}
