namespace CafeBazaar.DeveloperApi
{
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Olive;

    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddCafeBazaarDeveloperApi(this IServiceCollection services, string configKey = "CafeBazaar")
        {
            services.AddOptions<CafeBazaarOptions>()
                    .Configure<IConfiguration>((opts, config) => config.GetSection(configKey)?.Bind(opts))
                    .Validate(opts => opts.BaseUri is not null, $"{nameof(CafeBazaarOptions.BaseUri)} is null.")
                    .Validate(opts => !opts.BaseUri.IsAbsoluteUri, $"{nameof(CafeBazaarOptions.BaseUri)} is not absolute.")
                    .Validate(opts => opts.RedirectUri is not null, $"{nameof(CafeBazaarOptions.RedirectUri)} is null.")
                    .Validate(opts => opts.ClientId.HasValue(), $"{nameof(CafeBazaarOptions.ClientId)} is empty.")
                    .Validate(opts => opts.ClientSecret.HasValue(), $"{nameof(CafeBazaarOptions.ClientSecret)} is empty.")
                    .PostConfigure<IHttpContextAccessor>((opts, contextAccessor) =>
                    {
                        if (opts.RedirectUri.IsAbsoluteUri) return;
                        opts.RedirectUri = contextAccessor.ToAbsolute(opts.RedirectUri);
                    });

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
