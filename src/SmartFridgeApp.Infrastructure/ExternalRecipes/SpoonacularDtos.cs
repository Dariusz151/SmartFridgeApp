using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SmartFridgeApp.Infrastructure.ExternalRecipes;

internal class SpoonacularSearchResponse
{
    [JsonPropertyName("results")]
    public List<SpoonacularRecipeDto> Results { get; set; } = [];

    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("totalResults")]
    public int TotalResults { get; set; }
}

internal class SpoonacularRecipeDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("readyInMinutes")]
    public int ReadyInMinutes { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    [JsonPropertyName("dishTypes")]
    public List<string> DishTypes { get; set; } = [];

    [JsonPropertyName("extendedIngredients")]
    public List<SpoonacularIngredientDto> ExtendedIngredients { get; set; } = [];
}

internal class SpoonacularIngredientDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("amount")]
    public float Amount { get; set; }

    [JsonPropertyName("unit")]
    public string Unit { get; set; }

    [JsonPropertyName("measures")]
    public SpoonacularMeasuresDto Measures { get; set; }
}

internal class SpoonacularMeasuresDto
{
    [JsonPropertyName("metric")]
    public SpoonacularMetricDto Metric { get; set; }
}

internal class SpoonacularMetricDto
{
    [JsonPropertyName("amount")]
    public float Amount { get; set; }

    [JsonPropertyName("unitShort")]
    public string UnitShort { get; set; }

    [JsonPropertyName("unitLong")]
    public string UnitLong { get; set; }
}
