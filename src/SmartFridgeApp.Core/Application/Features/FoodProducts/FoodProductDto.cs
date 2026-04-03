using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Application.Features
{
    public class FoodProductDto
    {
        public int FoodProductId { get; set; }
        public string FoodProductName { get; set; }
        public string FoodProductCategory { get; set; }
        public int VariantCount { get; set; }
        public StorageLocation? DefaultStorageLocation { get; set; }
        public Unit? DefaultUnit { get; set; }
    }
}
