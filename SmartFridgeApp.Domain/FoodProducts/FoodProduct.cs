namespace SmartFridgeApp.Domain.FoodProducts
{
    public class FoodProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public FoodProduct()
        {
            
        }

        public FoodProduct(string name)
        {
            Name = name;
        }
    }
}
