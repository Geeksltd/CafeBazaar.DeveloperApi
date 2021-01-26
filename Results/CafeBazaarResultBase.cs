namespace CafeBazaar.DeveloperApi
{
    using System;
    using Olive;

    public abstract class CafeBazaarResultBase
    {
        public string Error { get; set; }
        public string ErrorDescription { get; set; }

        public bool Succeeded => Error.IsEmpty();
        public bool Failed => !Succeeded;

        public void EnsureSucceeded()
        {
            if (Failed)
                throw new Exception($"{Error}: {ErrorDescription}");
        }
    }
}
