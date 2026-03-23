using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFridgeApp.Core.Contracts;

public interface ITranslationService
{
    Task<string> TranslateAsync(string text, string sourceLang, string targetLang, CancellationToken ct = default);
    Task<List<string>> TranslateBatchAsync(IEnumerable<string> texts, string sourceLang, string targetLang, CancellationToken ct = default);
}
