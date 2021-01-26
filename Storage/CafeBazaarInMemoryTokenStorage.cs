namespace CafeBazaar.DeveloperApi
{
    using Olive;
    using System;
    using System.Threading.Tasks;

    public class CafeBazaarInMemoryTokenStorage : CafeBazaarTokenStorageBase
    {
        private readonly CafeBazaarToken _token;

        public CafeBazaarInMemoryTokenStorage() => _token = new CafeBazaarToken();

        public override Task Save(string accessToken, DateTimeOffset expiresIn, string refreshToken)
        {
            if (accessToken.IsEmpty()) throw new ArgumentNullException(nameof(accessToken));

            if (expiresIn < DateTimeOffset.Now) throw new ArgumentException("Already expired!", nameof(expiresIn));

            if (refreshToken.IsEmpty()) throw new ArgumentNullException(nameof(refreshToken));

            _token.AccessToken = accessToken;
            _token.ExpiresAt = expiresIn.DateTime;
            _token.RefreshToken = refreshToken;

            return Task.CompletedTask;
        }

        public override Task Renew(string accessToken, DateTimeOffset expiresIn)
        {
            if (accessToken.IsEmpty()) throw new ArgumentNullException(nameof(accessToken));

            if (expiresIn < DateTimeOffset.Now) throw new ArgumentException("Already expired!", nameof(expiresIn));

            _token.AccessToken = accessToken;
            _token.ExpiresAt = expiresIn.DateTime;

            return Task.CompletedTask;
        }

        protected override Task<CafeBazaarToken> GetToken() => Task.FromResult(_token);
    }
}
