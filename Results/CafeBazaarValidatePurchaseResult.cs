namespace CafeBazaar.DeveloperApi
{
    using System;

    public class CafeBazaarValidatePurchaseResult : CafeBazaarResultBase
    {
        public CafeBazaarConsumptionState ConsumptionState { get; set; }
        public CafeBazaarPurchaseState PurchaseState { get; set; }
        public string Kind { get; set; }
        public string DeveloperPayload { get; set; }
        public DateTimeOffset PurchaseTime { get; set; }
    }
}
