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

        public CafeBazaarAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context, IOptionsSnapshot<CafeBazaarOptions> options, CafeBazaarDeveloperService developerService)
        {
            var isRedirectUri = context.Request.Path.StartsWithSegments(options.Value.RedirectUri.AbsolutePath);
            var code = context.Request.Query["code"].FirstOrDefault();

            if (isRedirectUri && code.HasValue())
            {
                await developerService.HandleAuthorizationCallback(code);
                await context.Response.WriteAsync("Cafe Bazaar authorization callback executed.");
                return;
            }

            await _next(context);
        }
    }
}
