namespace CafeBazaar.DeveloperApi
{
    using System;

    public class CafeBazaarToken
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; }
    }
}
