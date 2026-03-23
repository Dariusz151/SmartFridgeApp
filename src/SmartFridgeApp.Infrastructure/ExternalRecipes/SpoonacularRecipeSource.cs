using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SmartFridgeApp.Core.Contracts.ExternalRecipes;

namespace SmartFridgeApp.Infrastructure.ExternalRecipes;

public class SpoonacularRecipeSource(HttpClient httpClient, IOptions<SpoonacularOptions> options)
    : IExternalRecipeSource
{
    public async Task<List<ExternalRecipe>> FetchRecipesAsync(int batchSize, CancellationToken ct = default)
    {
        var config = options.Value;
        var url = $"{config.BaseUrl}/recipes/complexSearch" +
                  $"?addRecipeInformation=true&fillIngredients=true&number={batchSize}&apiKey={config.ApiKey}";

        var response = await httpClient.GetFromJsonAsync<SpoonacularSearchResponse>(url, ct);

        if (response?.Results is null)
            return [];

        return response.Results.Select(MapToExternalRecipe).ToList();
    }

    private static ExternalRecipe MapToExternalRecipe(SpoonacularRecipeDto dto) => new()
    {
        Title = dto.Title ?? string.Empty,
        Description = StripHtmlTags(dto.Summary ?? string.Empty),
        ReadyInMinutes = dto.ReadyInMinutes > 0 ? dto.ReadyInMinutes : 1,
        DishTypes = dto.DishTypes ?? [],
        Ingredients = dto.ExtendedIngredients?
            .Where(i => !string.IsNullOrWhiteSpace(i.Name))
            .Select(MapToExternalIngredient)
            .ToList() ?? []
    };

    private static ExternalIngredient MapToExternalIngredient(SpoonacularIngredientDto dto)
    {
        var amount = dto.Measures?.Metric?.Amount ?? dto.Amount;
        var unit = dto.Measures?.Metric?.UnitShort ?? dto.Unit ?? string.Empty;

        return new ExternalIngredient
        {
            Name = dto.Name.Trim(),
            Amount = amount > 0 ? amount : 1,
            Unit = unit.ToLowerInvariant()
        };
    }

    private static string StripHtmlTags(string html)
    {
        if (string.IsNullOrEmpty(html)) return string.Empty;
        var text = Regex.Replace(html, "<.*?>", string.Empty);
        return text.Length > 5000 ? text[..5000] : text;
    }
}
