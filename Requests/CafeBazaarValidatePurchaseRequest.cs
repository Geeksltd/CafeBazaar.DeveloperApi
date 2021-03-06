﻿namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Olive;

    public class CafeBazaarValidatePurchaseRequest : ICafeBazaarRequest
    {
        /// <summary>
        /// Package name of the app in which product of the request purchase is defined.
        /// </summary>
        [JsonPropertyName("package_name")]
        public string PackageName { get; set; }

        /// <summary>
        /// SKU of the product.
        /// </summary>
        [JsonPropertyName("product_id")]
        public string ProductId { get; set; }

        /// <summary>
        /// Purchase token returned by Bazaar to app.
        /// </summary>
        [JsonPropertyName("purchase_token")]
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
