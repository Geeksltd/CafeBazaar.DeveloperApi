namespace CafeBazaar.DeveloperApi
{
    using System;

    public class CafeBazaarOptions
    {
        public Uri BaseUri { get; set; } = new Uri(@"https://pardakht.cafebazaar.ir");
        public string RedirectPath { get; set; } = "cafe-bazaar/auth-callback";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
