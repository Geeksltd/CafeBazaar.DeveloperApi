namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Threading.Tasks;

    public interface ICafeBazaarTokenStorage
    {
        Task<string> GetAccessToken();
        Task<(string, string)> GetTokenValue();
        Task<bool> AccessTokenExpired();
        Task<string> GetRefreshToken();

        Task Save(string accessToken, string tokenType, TimeSpan expiresIn, string refreshToken);
        Task Renew(string accessToken, string tokenType, TimeSpan expiresIn);
    }
}
