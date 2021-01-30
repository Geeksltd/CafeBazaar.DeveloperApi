# CafeBazaar.DeveloperApi

This is a simple C# library you can use to communicate with Cafe Bazaar's Developer APIs with ease.

## Registration

To use this library, you need to add its dependencies using `AddCafeBazaarDeveloperApi` method. First of all, add the required using statement.

```c#
using CafeBazaar.DeveloperApi;
```

Then make a call to `AddCafeBazaarDeveloperApi` extension method.

```c#
public override void ConfigureServices(IServiceCollection services)
{
    services.AddCafeBazaarDeveloperApi(Configuration);
}
```

This method will register an instance of the following classes.

`CafeBazaarOptions`: This is the options used in the library.

`CafeBazaarDeveloperService`: You will use this to call any of the provided endpoints.

`CafeBazaarInMemoryTokenStorage`: This is the default implementation of `ICafeBazaarTokenStorage`, which stores the authorization details in-memory. By using this, you'll have to re-authorize your app against Cafe Bazaar.

## Configuration

Here is the required properties:

```json
{
  "CafeBazaar": {
    "BaseUri": "https://pardakht.cafebazaar.ir", // This is optional
    "RedirectUri": "https://your-app.com:5000/cafe-bazaar-authorize-callback", // localhost isn't allowed by Cafe Bazaar
    "ClientId": "<YOUR_CLIENT_ID>",
    "ClientSecret": "<YOUR_CLIENT_SECRET>"
  }
}
```

## Authorize your app

You can use `IsAuthorizationRequired` to check whether authorization is required or not. If this method return true, you have to redirect to Cafe Bazaar's authorization page and authorize your app. `GetAuthorizationUri` should be used to get the authorization url. After user successfully authorized the app, Cafe Bazaar will redirect the user to our app, to where we've specified in our already created [Client](https://pishkhan.cafebazaar.ir/settings/api). There will be a query param with name `code` which contains an authorization code and you'll need to pass it to `HandleAuthorizationCallback`, which is responsible to complete the authorization steps and acquire the final access token from Cafe Bazaar. After a successful authorization, you can use provided services to validate a purchase, fetch details or cancel a subscription.

```c#
namespace MyApp
{
    using CafeBazaar.DeveloperApi;

    [ApiController]
    [Route("cafe-bazaar")]
    public class CafeBazaarController : ControllerBase
    {
        private readonly CafeBazaarDeveloperService _developerService;

        public CafeBazaarController(CafeBazaarDeveloperService developerService)
        {
            _developerService = developerService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            if (await _developerService.IsAuthorizationRequired())
                return Redirect(await _developerService.GetAuthorizationUri());

            return Content("Cafe Bazaar is authorized.");
        }

        [HttpGet("authorize-callback")]
        public async Task<IActionResult> AuthorizeCallback(string code)
        {
            await _developerService.HandleAuthorizationCallback(code);

            return RedirectToAction("Index");
        }
    }
}
```

### Refreshing access token

Cafe Bazaar's created access token only a valid for 1 hour, and after that, we'll need to obtain a new access token. This will be authomatically handled by the library and you don't have to do anything for this.

## Validate a purchase

You can use `ValidatePurchase` to validate a purchase token.

```c#
namespace MyApp
{
    using CafeBazaar.DeveloperApi;

    [ApiController]
    [Route("cafe-bazaar")]
    public class MyController : ControllerBase
    {
        private readonly CafeBazaarDeveloperService _developerService;

        public CafeBazaarController(CafeBazaarDeveloperService developerService)
        {
            _developerService = developerService;
        }

        [HttpGet("purchase/{productId}/{purchaseToken}")]
        public async Task<IActionResult> Validate(string productId, string purchaseToken)
        {
            var result = await _developerService.ValidatePurchase(new CafeBazaarValidatePurchaseRequest
            {
                PackageName = "my.app.com",
                ProductId = productId,
                PurchaseToken = purchaseToken
            });

            return new JsonResult(result);
        }
    }
}
```

## Validate a subscription

You can use `ValidateSubscription` to validate a subscription.

```c#
namespace MyApp
{
    using CafeBazaar.DeveloperApi;

    [ApiController]
    [Route("cafe-bazaar")]
    public class MyController : ControllerBase
    {
        private readonly CafeBazaarDeveloperService _developerService;

        public CafeBazaarController(CafeBazaarDeveloperService developerService)
        {
            _developerService = developerService;
        }

        [HttpGet("subscription/{subscriptionId}/{purchaseToken}")]
        public async Task<IActionResult> Validate(string subscriptionId, string purchaseToken)
        {
            var result = await _developerService.ValidateSubscription(new CafeBazaarValidateSubscriptionRequest
            {
                PackageName = "my.app.com",
                SubscriptionId = subscriptionId,
                PurchaseToken = purchaseToken
            });

            return new JsonResult(result);
        }
    }
}
```

## Cancel a subscription

You can use `CancelSubscription` to cancel a subscription.

```c#
namespace MyApp
{
    using CafeBazaar.DeveloperApi;

    [ApiController]
    [Route("cafe-bazaar")]
    public class MyController : ControllerBase
    {
        private readonly CafeBazaarDeveloperService _developerService;

        public CafeBazaarController(CafeBazaarDeveloperService developerService)
        {
            _developerService = developerService;
        }

        [HttpPost("subscription/{subscriptionId}/{purchaseToken}/cancel")]
        public async Task<IActionResult> CancelSubscription(string subscriptionId, string purchaseToken)
        {
            var result = await _developerService.CancelSubscription(new CafeBazaarCancelSubscriptionRequest
            {
                PackageName = "my.app.com",
                SubscriptionId = subscriptionId,
                PurchaseToken = purchaseToken
            });

            return new JsonResult(result);
        }
    }
}
```
