using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Application.Features
{
    public class AddFoodProductRequest
    {
        public string Name { get; set; }
        public int Category { get; set; }
        public StorageLocation? DefaultStorageLocation { get; set; }
        public Unit? DefaultUnit { get; set; }
    }
}
