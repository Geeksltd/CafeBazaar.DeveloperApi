namespace CafeBazaar.DeveloperApi
{
    using System;
    using Microsoft.AspNetCore.Http;

    static class HttpContextExtentions
    {
        public static Uri ToAbsolute(this IHttpContextAccessor contextAccessor, Uri relativeUri)
        {
            var request = contextAccessor.HttpContext.Request;
            return new Uri(new Uri($"{request.Scheme}://{request.Host}"), relativeUri);
        }
    }
}
