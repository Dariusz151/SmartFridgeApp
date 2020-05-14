namespace SmartFridgeApp.Domain.Models.FoodProducts
{
    public class Category
    {
        public short CategoryId { get; set; }
        public string Name { get; set; }

        public Category(string name)
        {
            Name = name;
        }

        public Category()
        {
                
        }
    }
}
