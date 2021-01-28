namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Text.Json.Serialization;

    public class CafeBazaarObtainTokenResult : CafeBazaarResultBase
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public string Scope { get; set; }
    }
}
