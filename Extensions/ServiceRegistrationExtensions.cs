namespace CafeBazaar.DeveloperApi
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Olive;

    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddCafeBazaarDeveloperApi(this IServiceCollection services, string configKey = "CafeBazaar")
        {
            services.AddOptions<CafeBazaarOptions>()
                    .Configure<IConfiguration>((opts, config) => config.GetSection(configKey)?.Bind(opts))
                    .Validate(opts => opts.BaseUri is not null, $"{nameof(CafeBazaarOptions.BaseUri)} is null.")
                    .Validate(opts => opts.BaseUri.IsAbsoluteUri, $"{nameof(CafeBazaarOptions.BaseUri)} is not absolute.")
                    .Validate(opts => opts.RedirectPath.HasValue(), $"{nameof(CafeBazaarOptions.RedirectPath)} is empty.")
                    .Validate(opts => opts.ClientId.HasValue(), $"{nameof(CafeBazaarOptions.ClientId)} is empty.")
                    .Validate(opts => opts.ClientSecret.HasValue(), $"{nameof(CafeBazaarOptions.ClientSecret)} is empty.")
                    .Validate(opts => opts.RefreshToken.HasValue(), $"{nameof(CafeBazaarOptions.RefreshToken)} is empty.");

            services.AddScoped<CafeBazaarDeveloperService>();

            services.AddSingleton<ICafeBazaarTokenStorage, CafeBazaarInMemoryTokenStorage>();

            return services;
        }
    }
}
