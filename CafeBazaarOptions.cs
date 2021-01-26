namespace CafeBazaar.DeveloperApi
{
    public class CafeBazaarOptions
    {
        public string BaseUri { get; set; } = "https://pardakht.cafebazaar.ir";
        public string RedirectUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
