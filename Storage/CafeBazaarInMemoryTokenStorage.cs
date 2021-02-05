namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Threading.Tasks;
    using Olive;

    public class CafeBazaarInMemoryTokenStorage : CafeBazaarTokenStorageBase
    {
        readonly CafeBazaarToken token;

        public CafeBazaarInMemoryTokenStorage() => token = new CafeBazaarToken();

        public override Task Save(string accessToken, TimeSpan expiresIn, string refreshToken)
        {
            if (accessToken.IsEmpty()) throw new ArgumentNullException(nameof(accessToken));

            if (expiresIn == TimeSpan.MinValue) throw new ArgumentException("Already expired!", nameof(expiresIn));

            if (refreshToken.IsEmpty()) throw new ArgumentNullException(nameof(refreshToken));

            token.AccessToken = accessToken;
            token.ExpiresAt = expiresIn.ToDateTime();
            token.RefreshToken = refreshToken;

            return Task.CompletedTask;
        }

        public override Task Renew(string accessToken, TimeSpan expiresIn)
        {
            if (accessToken.IsEmpty()) throw new ArgumentNullException(nameof(accessToken));

            if (expiresIn == TimeSpan.MinValue) throw new ArgumentException("Already expired!", nameof(expiresIn));

            token.AccessToken = accessToken;
            token.ExpiresAt = expiresIn.ToDateTime();

            return Task.CompletedTask;
        }

        protected override Task<CafeBazaarToken> GetToken() => Task.FromResult(token);
    }
}
