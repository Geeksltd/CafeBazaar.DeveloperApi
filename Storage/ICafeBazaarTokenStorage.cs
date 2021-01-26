namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Threading.Tasks;

    public interface ICafeBazaarTokenStorage
    {
        Task<string> GetAccessToken();
        Task<bool> AccessTokenExpired();
        Task<string> GetRefreshToken();

        Task Save(string accessToken, DateTimeOffset expiresIn, string refreshToken);
        Task Renew(string accessToken, DateTimeOffset expiresIn);
    }
}
