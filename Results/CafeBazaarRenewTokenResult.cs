namespace CafeBazaar.DeveloperApi
{
    using System;

    public class CafeBazaarRenewTokenResult : CafeBazaarResultBase
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public DateTimeOffset ExpiresIn { get; set; }
        public string Scope { get; set; }
    }
}
