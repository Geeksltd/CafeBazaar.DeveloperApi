namespace CafeBazaar.DeveloperApi
{
    using Olive;
    using System;

    public class CafeBazaarOptions
    {
        public Uri BaseUri { get; set; } = new Uri(@"https://pardakht.cafebazaar.ir");
        public Uri RedirectUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        internal bool Validate()
        {
            if (BaseUri == null) throw new ArgumentNullException(nameof(BaseUri));
            if (BaseUri.IsAbsoluteUri == false) throw new InvalidOperationException($"{nameof(BaseUri)} should be absolute.");

            if (RedirectUri == null) throw new ArgumentNullException(nameof(RedirectUri));
            if (RedirectUri.IsAbsoluteUri == false) throw new InvalidOperationException($"{nameof(RedirectUri)} should be absolute.");

            if (ClientId.IsEmpty()) throw new ArgumentNullException(nameof(ClientId));
            if (ClientSecret.IsEmpty()) throw new ArgumentNullException(nameof(ClientSecret));

            return true;
        }
    }
}
