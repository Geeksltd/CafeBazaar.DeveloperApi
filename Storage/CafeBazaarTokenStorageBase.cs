namespace CafeBazaar.DeveloperApi
{
    using Olive;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class CafeBazaarTokenStorageBase : ICafeBazaarTokenStorage
    {
        public async Task<string> GetAccessToken() => (await GetToken()).AccessToken;

        public async Task<(string, string)> GetTokenValue()
        {
            var token = await GetToken();
            return (token.TokenType, token.AccessToken);
        }

        public async Task<bool> AccessTokenExpired() => (await GetToken()).ExpiresAt < LocalTime.Now;

        public async Task<string> GetRefreshToken() => (await GetToken()).RefreshToken;

        public abstract Task Save(string accessToken, string tokenType, TimeSpan expiresIn, string refreshToken);

        public abstract Task Renew(string accessToken, string tokenType, TimeSpan expiresIn);

        protected abstract Task<CafeBazaarToken> GetToken();
    }
}
