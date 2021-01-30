namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Olive;

    public class CafeBazaarAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CafeBazaarOptions _options;

        public CafeBazaarAuthorizationMiddleware(RequestDelegate next, IOptions<CafeBazaarOptions> options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task InvokeAsync(HttpContext context, CafeBazaarDeveloperService developerService)
        {
            var isRedirectUri = context.Request.Path.StartsWithSegments(_options.RedirectUri.AbsolutePath);
            var code = context.Request.Query["code"].FirstOrDefault();

            if (isRedirectUri && code.HasValue())
            {
                await developerService.HandleAuthorizationCallback(code);
                await context.Response.WriteAsync("Cafe Bazaar authorization callback executed.");
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
