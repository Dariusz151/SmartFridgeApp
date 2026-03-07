using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SmartFridgeApp.API.Configuration
{
    public static class GoogleConfiguration
    {
        public static void ConfigureGoogle(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication()
                .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
                {
                    options.ClientId = configuration["Google:ClientId"];
                    options.ClientSecret = configuration["Google:ClientSecret"];
                    options.CallbackPath = "/api/auth/google-callback";
                    // Use the cookie scheme for the OAuth handshake sign-in step
                    options.SignInScheme = "Cookies";
                });
        }
    }
}
