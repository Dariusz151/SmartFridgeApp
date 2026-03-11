using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SmartFridgeApp.API.Configuration;

public static class RateLimitConfiguration
{
    public static void ConfigureRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        // Load general rules from appsettings.json → "IpRateLimiting"
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

        // Use in-memory stores (no Redis needed)
        services.AddMemoryCache();
        services.AddSingleton<IRateLimitConfiguration, AspNetCoreRateLimit.RateLimitConfiguration>();
        services.AddInMemoryRateLimiting();
    }
}
