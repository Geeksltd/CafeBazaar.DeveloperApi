namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Text.Json.Serialization;
    using Olive;

    public abstract class CafeBazaarResultBase
    {
        [JsonPropertyName("error")]
        public string Error { private get; set; }

        [JsonPropertyName("error_description")]
        public string ErrorDescription { private get; set; }

        bool Failed => Error.HasValue();

        public void EnsureSucceeded()
        {
            if (Failed)
                throw new Exception($"{Error}: {ErrorDescription}");
        }
    }
}
