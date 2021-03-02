namespace CafeBazaar.DeveloperApi
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;
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
                    .Validate(opts => opts.RedirectPath.HasValue(), $"{nameof(CafeBazaarOptions.RedirectPath)} is empty.")
                    .Validate(opts => opts.ClientId.HasValue(), $"{nameof(CafeBazaarOptions.ClientId)} is empty.")
                    .Validate(opts => opts.ClientSecret.HasValue(), $"{nameof(CafeBazaarOptions.ClientSecret)} is empty.");

            services.AddScoped<CafeBazaarDeveloperService>();

            services.AddSingleton<ICafeBazaarTokenStorage, CafeBazaarInMemoryTokenStorage>();

            return services;
        }

        public static IApplicationBuilder UseCafeBazaarDeveloperApi(this IApplicationBuilder builder)
        {
            var routes = new RouteBuilder(builder);

            var options = builder.ApplicationServices.GetService<IOptions<CafeBazaarOptions>>();

            routes.MapMiddlewarePost(options.Value.RedirectPath, builder => builder.UseMiddleware<CafeBazaarAuthorizationMiddleware>());

            builder.UseRouter(routes.Build());

            return builder;
        }
    }
}
