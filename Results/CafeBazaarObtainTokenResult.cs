namespace CafeBazaar.DeveloperApi
{
    using System;

    public class CafeBazaarObtainTokenResult : CafeBazaarResultBase
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public DateTimeOffset ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public string Scope { get; set; }
    }
}
