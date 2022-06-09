namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;

    public class CafeBazaarDeveloperService
    {
        readonly CafeBazaarOptions Options;
        readonly ICafeBazaarTokenStorage TokenStorage;
        readonly WebApiInvoker WebApiInvoker;

        public CafeBazaarDeveloperService(
            IOptionsSnapshot<CafeBazaarOptions> options,
            ICafeBazaarTokenStorage tokenStorage
        )
        {
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            TokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
            WebApiInvoker = new WebApiInvoker(Options.BaseUri);
        }

        public async Task<CafeBazaarValidatePurchaseResult> ValidatePurchase(CafeBazaarValidatePurchaseRequest request)
        {
            await request.Validate();

            await EnsureAccessTokenValidity();

            var path = $"devapi/v2/api/validate/{request.PackageName}/inapp/{request.ProductId}/purchases/{request.PurchaseToken}/?access_token={await TokenStorage.GetAccessToken()}";

            var result = await WebApiInvoker.Get<CafeBazaarValidatePurchaseResult>(path);

            result.EnsureSucceeded();

            return result;
        }

        public async Task<CafeBazaarValidateSubscriptionResult> ValidateSubscription(CafeBazaarValidateSubscriptionRequest request)
        {
            await request.Validate();

            await EnsureAccessTokenValidity();

            var path = $"devapi/v2/api/applications/{request.PackageName}/subscriptions/{request.SubscriptionId}/purchases/{request.PurchaseToken}/?access_token={await TokenStorage.GetAccessToken()}";

            var result = await WebApiInvoker.Get<CafeBazaarValidateSubscriptionResult>(path);

            result.EnsureSucceeded();

            return result;
        }

        public async Task<CafeBazaarCancelSubscriptionResult> CancelSubscription(CafeBazaarCancelSubscriptionRequest request)
        {
            await request.Validate();

            await EnsureAccessTokenValidity();

            var path = $"devapi/v2/api/applications/{request.PackageName}/subscriptions/{request.SubscriptionId}/purchases/{request.PurchaseToken}/cancel/?access_token={await TokenStorage.GetAccessToken()}";

            var result = await WebApiInvoker.Get<CafeBazaarCancelSubscriptionResult>(path);

            result.EnsureSucceeded();

            return result;
        }

        async Task EnsureAccessTokenValidity()
        {
            if (await TokenStorage.AccessTokenExpired())
                await RenewAccessToken();
        }

        async Task RenewAccessToken()
        {
            var path = "devapi/v2/auth/token/";

            var request = new CafeBazaarRenewTokenRequest
            {
                ClientId = Options.ClientId,
                ClientSecret = Options.ClientSecret,
                RefreshToken = Options.RefreshToken
            };

            await request.Validate();

            var result = await WebApiInvoker.PostForm<CafeBazaarRenewTokenResult>(path, request);

            result.EnsureSucceeded();

            await TokenStorage.Renew(result.AccessToken, result.ExpiresIn);
        }
    }
}
