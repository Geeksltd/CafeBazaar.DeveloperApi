namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Threading.Tasks;

    public interface ICafeBazaarTokenStorage
    {
        Task<string> GetAccessToken();
        Task<bool> AccessTokenExpired();

        Task Renew(string accessToken, TimeSpan expiresIn);
    }
}
