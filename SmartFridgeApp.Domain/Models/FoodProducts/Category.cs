namespace SmartFridgeApp.Domain.Models.FoodProducts
{
    public class Category
    {
        public string Name { get; set; }
        public short CategoryId { get; set; }

        public Category(string name)
        {
            Name = name;
        }

        public Category()
        {
                
        }
    }
}
