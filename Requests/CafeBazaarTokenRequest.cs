namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Threading.Tasks;
    using Olive;

    public class CafeBazaarRenewTokenRequest : ICafeBazaarRequest
    {
        public string GrantType => "refresh_token";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RefreshToken { get; set; }

        public Task Validate()
        {
            if (ClientId.IsEmpty()) throw new ArgumentNullException(nameof(ClientId));

            if (ClientSecret.IsEmpty()) throw new ArgumentNullException(nameof(ClientSecret));

            if (RefreshToken.IsEmpty()) throw new ArgumentNullException(nameof(RefreshToken));

            return Task.CompletedTask;
        }
    }
}
