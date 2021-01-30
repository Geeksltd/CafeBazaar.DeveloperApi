namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Olive;

    class CafeBazaarObtainTokenRequest : ICafeBazaarRequest
    {
        [JsonPropertyName("grant_type")]
        public string GrantType => "authorization_code";

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("redirect_uri")]
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
