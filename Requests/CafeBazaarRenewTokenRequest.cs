namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Olive;

    class CafeBazaarRenewTokenRequest : ICafeBazaarRequest
    {
        [JsonPropertyName("grant_type")]
        public string GrantType => "refresh_token";

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("refresh_token")]
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
