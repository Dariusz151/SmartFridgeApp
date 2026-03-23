namespace SmartFridgeApp.Infrastructure.Translation;

public class TranslationOptions
{
    /// <summary>LibreTranslate instance URL, e.g. https://libretranslate.com</summary>
    public string BaseUrl { get; set; } = "https://libretranslate.com";

    /// <summary>Optional API key (required for hosted instances with rate limits).</summary>
    public string? ApiKey { get; set; }
}
