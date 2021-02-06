namespace CafeBazaar.DeveloperApi
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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
            return builder.UseMiddleware<CafeBazaarAuthorizationMiddleware>();
        }
    }
}
