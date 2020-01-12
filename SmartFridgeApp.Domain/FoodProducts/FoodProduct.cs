namespace SmartFridgeApp.Domain.FoodProducts
{
    public class FoodProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public FoodProduct(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
