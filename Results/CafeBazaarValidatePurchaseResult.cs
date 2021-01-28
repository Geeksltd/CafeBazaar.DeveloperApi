namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Text.Json.Serialization;

    public class CafeBazaarValidatePurchaseResult : CafeBazaarResultBase
    {
        public CafeBazaarConsumptionState ConsumptionState { get; set; }
        public CafeBazaarPurchaseState PurchaseState { get; set; }
        public string Kind { get; set; }
        public string DeveloperPayload { get; set; }
        [JsonConverter(typeof(DateTimeOffsetConverter))]
        public DateTimeOffset PurchaseTime { get; set; }
    }
}
