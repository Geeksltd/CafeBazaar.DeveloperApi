namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Text.Json.Serialization;

    public class CafeBazaarValidateSubscriptionResult : CafeBazaarResultBase
    {
        /// <summary>
        /// Type of the returned resource. This will always be androidpublisher#subscriptionPurchase
        /// </summary>
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        /// <summary>
        /// Time of the initiation of subscription.
        /// </summary>
        [JsonPropertyName("initiationTimestampMsec")]
        [JsonConverter(typeof(DateTimeOffsetConverter))]
        public DateTimeOffset InitiationTime { get; set; }

        /// <summary>
        /// Time of the next anticipated billing. For subscriptions that don't automatically renew, this is when the subscription ends.
        /// </summary>
        [JsonPropertyName("validUntilTimestampMsec")]
        [JsonConverter(typeof(DateTimeOffsetConverter))]
        public DateTimeOffset ValidUntil { get; set; }

        /// <summary>
        /// A boolean value representing whether next billings occur automatically or not.
        /// </summary>
        [JsonPropertyName("autoRenewing")]
        public bool AutoRenewing { get; set; }
    }
}
