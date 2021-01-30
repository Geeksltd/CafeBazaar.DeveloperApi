namespace CafeBazaar.DeveloperApi
{
    using System;

    public class CafeBazaarOptions
    {
        public Uri BaseUri { get; set; } = new Uri(@"https://pardakht.cafebazaar.ir");
        public Uri RedirectUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
