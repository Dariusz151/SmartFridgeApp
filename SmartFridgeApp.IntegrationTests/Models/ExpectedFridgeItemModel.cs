using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.IntegrationTests.Models
{
    public class ExpectedFridgeItemModel
    {
        public long Id { get; private set; }
        public int FoodProductId { get; private set; }
        public string Note { get; private set; }
        public AmountValue AmountValue { get; private set; }
    }
}
