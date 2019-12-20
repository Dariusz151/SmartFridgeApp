using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.FridgeItems.AddFridgeItem
{
    public class AddFridgeItemRequest
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public AmountValue AmountValue { get; set; }

    }
}
