namespace CafeBazaar.DeveloperApi
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Olive;
    using System.Linq;

    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddCafeBazaarDeveloperApi(this IServiceCollection services, string configKey = "CafeBazaar")
        {
            services.AddOptions<CafeBazaarOptions>()
                    .Configure<IConfiguration>((opts, config) => config.GetSection(configKey)?.Bind(opts))
                    .Validate(opts => opts.Validate());

            services.AddScoped<CafeBazaarDeveloperService>();

            services.AddSingleton<ICafeBazaarTokenStorage, CafeBazaarInMemoryTokenStorage>();

            return services;
        }

        public static IApplicationBuilder UseCafeBazaarDeveloperApi(this IApplicationBuilder builder)
        {
            return builder.MapWhen(MatchesRedirectUriEndpoint, builder => builder.UseMiddleware<CafeBazaarAuthorizationMiddleware>());
        }

        static bool MatchesRedirectUriEndpoint(HttpContext context)
        {
            var options = context.RequestServices.GetService<IOptionsSnapshot<CafeBazaarOptions>>();

            if (!context.Request.Path.StartsWithSegments(options.Value.RedirectUri.AbsolutePath)) return false;

            if (context.Request.Query["code"].FirstOrDefault() is null) return false;

            return true;
        }
    }
}
