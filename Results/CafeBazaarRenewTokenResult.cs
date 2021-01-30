namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Text.Json.Serialization;

    public class CafeBazaarRenewTokenResult : CafeBazaarResultBase
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ExpiresIn { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
