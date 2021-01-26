# CafeBazaar.DeveloperApi

This is a simple C# library you can use to communicate with Cafe Bazaar's Developer APIs with ease.

## Registration

To use this library, you need to add its dependencies using `AddCafeBazaarDeveloperApi` method. First of all, add the required using statement.

```
using CafeBazaar.DeveloperApi;
```

Then make a call to `AddCafeBazaarDeveloperApi` extension method.

```

public override void ConfigureServices(IServiceCollection services)
{
	services.AddCafeBazaarDeveloperApi(Configuration);
}
```

This method will register an instance of the following classes.

`CafeBazaarDeveloperService`: You will use this to call any of the provided endpoints.

## Configuration

Here is the required properties:

```
{
  "CafeBazaar": {
    "BaseUri": "https://pardakht.cafebazaar.ir", // This is optional
    "RedirectUri": "https://your-app.com:5000/cafe-bazaar-authorize-callback", // localhost isn't allowed by Cafe Bazaar
    "ClientId": "<YOUR_CLIENT_ID>",
    "ClientSecret": "<YOUR_CLIENT_SECRET>"
  }
}
```

## Validate a purchase token

You can use `ValidatePurchase` to validate a purchase token.

```
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

        [HttpGet("validate/{token}")]
        public async Task<IActionResult> Validate(string productId, string purchaseToken)
        {
            var result = await _developerService.ValidatePurchase(new CafeBazaarValidatePurchaseRequest
            {
                PackageName = "my.app.com",
                ProductId = productId,
                PurchaseToken = purchaseToken
            });

            if (result == null) return Content("Not found");
            return new JsonResult(result);
        }
    }
}
```