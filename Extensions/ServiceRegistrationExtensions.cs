namespace CafeBazaar.DeveloperApi
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddCafeBazaarDeveloperApi(this IServiceCollection services, IConfiguration config, string configKey = "CafeBazaar")
        {
            return services.Configure<CafeBazaarOptions>(opts => config.GetSection(configKey)?.Bind(opts))
                           .AddScoped<CafeBazaarDeveloperService>()
                           .AddSingleton<ICafeBazaarTokenStorage, CafeBazaarInMemoryTokenStorage>();
        }
    }
}
