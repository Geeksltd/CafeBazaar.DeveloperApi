namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Text.Json.Serialization;

    public class CafeBazaarValidatePurchaseResult : CafeBazaarResultBase
    {
        /// <summary>
        /// The consumption state of requested purchase.
        /// </summary>
        [JsonPropertyName("consumptionState")]
        public CafeBazaarConsumptionState ConsumptionState { get; set; }

        /// <summary>
        /// The purchase state of requested purchase.
        /// </summary>
        [JsonPropertyName("purchaseState")]
        public CafeBazaarPurchaseState PurchaseState { get; set; }

        /// <summary>
        /// Type of the returned resource. This will always be androidpublisher#inappPurchase
        /// </summary>
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        /// <summary>
        /// The payload string that is sent to Bazaar by app when making the purchase.
        /// </summary>
        [JsonPropertyName("developerPayload")]
        public string DeveloperPayload { get; set; }

        /// <summary>
        /// Time of the purchase.
        /// </summary>
        [JsonPropertyName("purchaseTime")]
        [JsonConverter(typeof(DateTimeOffsetConverter))]
        public DateTimeOffset PurchaseTime { get; set; }
    }
}
