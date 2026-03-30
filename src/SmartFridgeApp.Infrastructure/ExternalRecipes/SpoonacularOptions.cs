namespace SmartFridgeApp.Infrastructure.ExternalRecipes;

public class SpoonacularOptions
{
    public required string ApiKey { get; set; }
    public string BaseUrl { get; set; } = "https://api.spoonacular.com";
}
