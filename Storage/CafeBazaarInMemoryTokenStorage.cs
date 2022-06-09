namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Threading.Tasks;
    using Olive;

    public class CafeBazaarInMemoryTokenStorage : CafeBazaarTokenStorageBase
    {
        readonly CafeBazaarToken Token;

        public CafeBazaarInMemoryTokenStorage() => Token = new CafeBazaarToken();

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
