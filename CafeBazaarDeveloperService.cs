namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Olive;

    public class CafeBazaarDeveloperService
    {
        readonly CafeBazaarOptions options;
        readonly ICafeBazaarTokenStorage tokenStorage;
        readonly WebApiInvoker webApiInvoker;

        public CafeBazaarDeveloperService(IOptionsSnapshot<CafeBazaarOptions> options, ICafeBazaarTokenStorage tokenStorage)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.tokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
            this.webApiInvoker = new WebApiInvoker(this.options.BaseUri);
        }

        public async Task<bool> IsAuthorizationRequired()
        {
            return (await tokenStorage.GetAccessToken()).IsEmpty();
        }

        public Task<string> GetAuthorizationUri()
        {
            return Task.FromResult(
                $"{options.BaseUri}devapi/v2/auth/authorize/?response_type=code&access_type=offline&redirect_uri={options.RedirectUri}&client_id={options.ClientId}"
            );
        }

        public async Task HandleAuthorizationCallback(string code)
        {
            var path = "devapi/v2/auth/token/";

            var request = new CafeBazaarObtainTokenRequest
            {
                Code = code,
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret,
                RedirectUri = options.RedirectUri.ToString()
            };

            await request.Validate();

            var result = await webApiInvoker.PostForm<CafeBazaarObtainTokenResult>(path, request);

            result.EnsureSucceeded();

            await tokenStorage.Save(result.AccessToken, result.ExpiresIn, result.RefreshToken);
        }

        public async Task<CafeBazaarValidatePurchaseResult> ValidatePurchase(CafeBazaarValidatePurchaseRequest request)
        {
            await request.Validate();

            await EnsureAccessTokenValidity();

            var path = $"devapi/v2/api/validate/{request.PackageName}/inapp/{request.ProductId}/purchases/{request.PurchaseToken}/?access_token={await tokenStorage.GetAccessToken()}";

            var result = await webApiInvoker.Get<CafeBazaarValidatePurchaseResult>(path);

            result.EnsureSucceeded();

            return result;
        }

        public async Task<CafeBazaarValidateSubscriptionResult> ValidateSubscription(CafeBazaarValidateSubscriptionRequest request)
        {
            await request.Validate();

            await EnsureAccessTokenValidity();

            var path = $"devapi/v2/api/applications/{request.PackageName}/subscriptions/{request.SubscriptionId}/purchases/{request.PurchaseToken}/?access_token={await tokenStorage.GetAccessToken()}";

            var result = await webApiInvoker.Get<CafeBazaarValidateSubscriptionResult>(path);

            result.EnsureSucceeded();

            return result;
        }

        public async Task<CafeBazaarCancelSubscriptionResult> CancelSubscription(CafeBazaarCancelSubscriptionRequest request)
        {
            await request.Validate();

            await EnsureAccessTokenValidity();

            var path = $"devapi/v2/api/applications/{request.PackageName}/subscriptions/{request.SubscriptionId}/purchases/{request.PurchaseToken}/cancel/?access_token={await tokenStorage.GetAccessToken()}";

            var result = await webApiInvoker.Get<CafeBazaarCancelSubscriptionResult>(path);

            result.EnsureSucceeded();

            return result;
        }

        async Task EnsureAccessTokenValidity()
        {
            if ((await tokenStorage.GetRefreshToken()).IsEmpty())
                throw new Exception("First of all you need to authorize against Cafe Bazzar.");

            if (await tokenStorage.AccessTokenExpired())
                await RenewAccessToken();
        }

        async Task RenewAccessToken()
        {
            var path = "devapi/v2/auth/token/";

            var request = new CafeBazaarRenewTokenRequest
            {
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret,
                RefreshToken = await tokenStorage.GetRefreshToken()
            };

            await request.Validate();

            var result = await webApiInvoker.PostForm<CafeBazaarRenewTokenResult>(path, request);

            result.EnsureSucceeded();

            await tokenStorage.Renew(result.AccessToken, result.ExpiresIn);
        }
    }
}
