namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Threading.Tasks;
    using Olive;

    public class CafeBazaarObtainTokenRequest : ICafeBazaarRequest
    {
        public string GrantType => "authorization_code";
        public string Code { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }

        public Task Validate()
        {
            if (Code.IsEmpty()) throw new ArgumentNullException(nameof(Code));

            if (ClientId.IsEmpty()) throw new ArgumentNullException(nameof(ClientId));

            if (ClientSecret.IsEmpty()) throw new ArgumentNullException(nameof(ClientSecret));

            if (RedirectUri.IsEmpty()) throw new ArgumentNullException(nameof(RedirectUri));

            return Task.CompletedTask;
        }
    }
}
