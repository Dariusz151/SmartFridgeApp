namespace SmartFridgeApp.Core.Domain.Entities
{
    public class RecipeCategory
    {
        public string Name { get; set; }
        public short RecipeCategoryId { get; set; }

        public RecipeCategory(string name)
        {
            Name = name;
        }

        public RecipeCategory()
        {

        }
    }
}
