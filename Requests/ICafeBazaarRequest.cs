namespace CafeBazaar.DeveloperApi
{
    using System.Threading.Tasks;

    public interface ICafeBazaarRequest
    {
        Task Validate();
    }
}
