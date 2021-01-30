namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Olive;

    public class CafeBazaarCancelSubscriptionRequest : ICafeBazaarRequest
    {
        /// <summary>
        /// Package name of the app in which product of the request subscription is defined.
        /// </summary>
        [JsonPropertyName("grant_type")]
        public string PackageName { get; set; }

        /// <summary>
        /// SKU of the subscription product.
        /// </summary>
        [JsonPropertyName("grant_type")]
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Purchase token returned by Bazaar to app when subscription started. You can also use the token of any of the further billings of that subscription.
        /// </summary>
        [JsonPropertyName("grant_type")]
        public string PurchaseToken { get; set; }

        public Task Validate()
        {
            if (PackageName.IsEmpty()) throw new ArgumentNullException(nameof(PackageName));

            if (SubscriptionId.IsEmpty()) throw new ArgumentNullException(nameof(SubscriptionId));

            if (PurchaseToken.IsEmpty()) throw new ArgumentNullException(nameof(PurchaseToken));

            return Task.CompletedTask;
        }
    }
}
