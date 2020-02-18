namespace SmartFridgeApp.Domain.Models.FoodProducts
{
    public class Category
    {
        public byte CategoryId { get; set; }
        public string Name { get; set; }

        public Category(string name)
        {
            Name = name;
        }
    }
}
