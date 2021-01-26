namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Threading.Tasks;
    using Olive;

    public class CafeBazaarValidatePurchaseRequest : ICafeBazaarRequest
    {
        public string PackageName { get; set; }
        public string ProductId { get; set; }
        public string PurchaseToken { get; set; }

        public Task Validate()
        {
            if (PackageName.IsEmpty()) throw new ArgumentNullException(nameof(PackageName));

            if (ProductId.IsEmpty()) throw new ArgumentNullException(nameof(ProductId));

            if (PurchaseToken.IsEmpty()) throw new ArgumentNullException(nameof(PurchaseToken));

            return Task.CompletedTask;
        }
    }
}
