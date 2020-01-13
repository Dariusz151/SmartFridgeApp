using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.FoodProducts
{
    public class FoodProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public FoodProduct()
        {
            // EF Core
        }

        public FoodProduct(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new DomainException("Product name can't be empty.");
            Name = name;
        }
    }
}
