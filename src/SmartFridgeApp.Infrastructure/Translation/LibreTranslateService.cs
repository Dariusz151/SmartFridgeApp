using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartFridgeApp.Core.Contracts;

namespace SmartFridgeApp.Infrastructure.Translation;

public class LibreTranslateService(
    HttpClient httpClient,
    IOptions<TranslationOptions> options,
    ILogger<LibreTranslateService> logger) : ITranslationService
{
    public async Task<string> TranslateAsync(string text, string sourceLang, string targetLang, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(text)) return text;

        var config = options.Value;
        var payload = new Dictionary<string, string>
        {
            ["q"] = text,
            ["source"] = sourceLang,
            ["target"] = targetLang,
            ["format"] = "text"
        };
        if (!string.IsNullOrEmpty(config.ApiKey))
            payload["api_key"] = config.ApiKey;

        try
        {
            var response = await httpClient.PostAsJsonAsync($"{config.BaseUrl}/translate", payload, ct);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TranslateResponse>(cancellationToken: ct);
            return result?.TranslatedText ?? text;
        }
        catch (System.Exception ex)
        {
            logger.LogWarning(ex, "[Translation] Failed to translate '{Text}', returning original", text);
            return text;
        }
    }

    public async Task<List<string>> TranslateBatchAsync(IEnumerable<string> texts, string sourceLang, string targetLang, CancellationToken ct = default)
    {
        var items = texts.ToList();
        if (items.Count == 0) return [];

        // LibreTranslate supports single text per request; batch by joining with a separator and splitting.
        // Use newline separator — works well for short ingredient names / recipe titles.
        var joined = string.Join("\n", items);

        var translated = await TranslateAsync(joined, sourceLang, targetLang, ct);
        var parts = translated.Split('\n');

        // If split count doesn't match, fall back to originals for missing entries.
        return items.Select((original, i) => i < parts.Length ? parts[i].Trim() : original).ToList();
    }

    private sealed class TranslateResponse
    {
        [JsonPropertyName("translatedText")]
        public string? TranslatedText { get; set; }
    }
}
