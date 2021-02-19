namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Olive;

    class CafeBazaarAuthorizationMiddleware
    {
        readonly RequestDelegate Next;

        public CafeBazaarAuthorizationMiddleware(RequestDelegate next)
        {
            Next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context, CafeBazaarDeveloperService developerService)
        {
            var code = context.Request.Query["code"].FirstOrDefault();

            await developerService.HandleAuthorizationCallback(code);
            await context.Response.WriteAsync("Cafe Bazaar authorization callback executed.");

            await Next(context);
        }
    }
}
