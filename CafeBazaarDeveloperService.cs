namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Olive;

    public class CafeBazaarDeveloperService
    {
        private readonly CafeBazaarOptions _options;
        private readonly ICafeBazaarTokenStorage _tokenStorage;

        public CafeBazaarDeveloperService(IOptions<CafeBazaarOptions> options, ICafeBazaarTokenStorage tokenStorage)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _tokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
        }

        public async Task<bool> IsAuthorizationRequired()
        {
            return (await _tokenStorage.GetAccessToken()).IsEmpty();
        }

        public Task<string> GetAuthorizationUri()
        {
            return Task.FromResult(
                $"{_options.BaseUri}/devapi/v2/auth/authorize/?response_type=code&access_type=offline&redirect_uri={_options.RedirectUri}&client_id={_options.ClientId}"
            );
        }

        public async Task HandleAuthorizationCallback(string code)
        {
            var path = "/devapi/v2/auth/token/";

            var request = new CafeBazaarObtainTokenRequest
            {
                Code = code,
                ClientId = _options.ClientId,
                ClientSecret = _options.ClientSecret,
                RedirectUri = _options.RedirectUri
            };

            await request.Validate();

            var result = await _options.BaseUri.Post<CafeBazaarObtainTokenResult>(path, request.ToDictionary());

            result.EnsureSucceeded();

            await _tokenStorage.Save(result.AccessToken, result.ExpiresIn, result.RefreshToken);
        }

        public async Task<CafeBazaarValidatePurchaseResult> ValidatePurchase(CafeBazaarValidatePurchaseRequest request)
        {
            await request.Validate();

            await EnsureAccessTokenValidity();

            var path = $"/devapi/v2/api/validate/{request.PackageName}/inapp/{request.ProductId}/purchases/{request.PurchaseToken}/";

            var result = await _options.BaseUri.Get<CafeBazaarValidatePurchaseResult>(path);

            result.EnsureSucceeded();

            return result;
        }

        async Task EnsureAccessTokenValidity()
        {
            if ((await _tokenStorage.GetRefreshToken()).IsEmpty())
                throw new Exception("First of all you need to authorize against Cafe Bazzar.");

            if (await _tokenStorage.AccessTokenExpired())
                await RenewAccessToken();
        }

        async Task RenewAccessToken()
        {
            var path = "/devapi/v2/auth/token/";

            var request = new CafeBazaarRenewTokenRequest
            {
                ClientId = _options.ClientId,
                ClientSecret = _options.ClientSecret,
                RefreshToken = await _tokenStorage.GetRefreshToken()
            };

            await request.Validate();

            var result = await _options.BaseUri.Post<CafeBazaarRenewTokenResult>(path, request.ToDictionary());

            result.EnsureSucceeded();

            await _tokenStorage.Renew(result.AccessToken, result.ExpiresIn);
        }
    }
}
