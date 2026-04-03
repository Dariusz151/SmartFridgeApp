using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Application.Features
{
    public class UpdateFoodProductRequest
    {
        public int FoodProductId { get; set; }
        public string FoodProductName { get; set; }
        public StorageLocation? DefaultStorageLocation { get; set; }
        public Unit? DefaultUnit { get; set; }
    }
}
