namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Threading.Tasks;
    using Olive;

    public class CafeBazaarInMemoryTokenStorage : CafeBazaarTokenStorageBase
    {
        readonly CafeBazaarToken Token;

        public CafeBazaarInMemoryTokenStorage() => Token = new CafeBazaarToken();

        public override Task Save(string accessToken, TimeSpan expiresIn, string refreshToken)
        {
            if (accessToken.IsEmpty()) throw new ArgumentNullException(nameof(accessToken));

            if (expiresIn == TimeSpan.MinValue) throw new ArgumentException("Already expired!", nameof(expiresIn));

            if (refreshToken.IsEmpty()) throw new ArgumentNullException(nameof(refreshToken));

            Token.AccessToken = accessToken;
            Token.ExpiresAt = expiresIn.ToDateTime();
            Token.RefreshToken = refreshToken;

            return Task.CompletedTask;
        }

        public override Task Renew(string accessToken, TimeSpan expiresIn)
        {
            if (accessToken.IsEmpty()) throw new ArgumentNullException(nameof(accessToken));

            if (expiresIn == TimeSpan.MinValue) throw new ArgumentException("Already expired!", nameof(expiresIn));

            Token.AccessToken = accessToken;
            Token.ExpiresAt = expiresIn.ToDateTime();

            return Task.CompletedTask;
        }

        protected override Task<CafeBazaarToken> GetToken() => Task.FromResult(Token);
    }
}
