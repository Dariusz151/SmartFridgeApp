namespace SmartFridgeApp.API.Recipes.UpdateRecipe
{
    public class UpdateRecipeRequest
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}
