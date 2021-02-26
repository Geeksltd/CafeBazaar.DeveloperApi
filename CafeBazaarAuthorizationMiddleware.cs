namespace CafeBazaar.DeveloperApi
{
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Olive;

    class CafeBazaarAuthorizationMiddleware
    {
        public CafeBazaarAuthorizationMiddleware(RequestDelegate _) { }

        public async Task InvokeAsync(HttpContext context, CafeBazaarDeveloperService developerService)
        {
            var code = context.Request.Query["code"].FirstOrDefault();

            await developerService.HandleAuthorizationCallback(code);

            await context.Response.WriteAsync("Cafe Bazaar authorization callback executed.");
        }
    }
}
