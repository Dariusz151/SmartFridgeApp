namespace SmartFridgeApp.API.Recipes
{
    public class RecipeDto
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string Description { get; set; }
        public int DifficultyLevel { get; set; }
        public int MinutesRequired { get; set; }
        public string Category { get; set; }
        // TODO: how to return this serialized string?
        public string FoodProducts { get; set; }
    }
}
