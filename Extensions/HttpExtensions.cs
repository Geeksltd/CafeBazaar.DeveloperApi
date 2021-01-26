namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Olive;

    static class HttpExtensions
    {
        public static Task<T> Get<T>(this string baseAddress, string path, Encoding encoding = null) where T : CafeBazaarResultBase, new()
        {
            return Send<T>(baseAddress, (client, _) => client.GetAsync(path), encoding);
        }

        public static Task<T> Post<T>(this string baseAddress, string path, object request, Encoding encoding = null) where T : CafeBazaarResultBase, new()
        {
            return Send<T>(baseAddress, async (client, enc) =>
            {
                var payload = new StringContent(request.ToJson(), encoding, "text/json");
                return await client.PostAsync(path, payload);
            }, encoding);
        }

        public static Task<T> Post<T>(this string baseAddress, string path, IDictionary<string, string> request, Encoding encoding = null) where T : CafeBazaarResultBase, new()
        {
            return Send<T>(baseAddress, async (client, enc) =>
            {
                var payload = new FormUrlEncodedContent(request);
                return await client.PostAsync(path, payload);
            }, encoding);
        }

        static async Task<T> Send<T>(string baseAddress, Func<HttpClient, Encoding, Task<HttpResponseMessage>> requestInitiator, Encoding encoding) where T : CafeBazaarResultBase, new()
        {
            encoding ??= Encoding.UTF8;

            try
            {
                var client = CreateClient(baseAddress);

                var message = await requestInitiator(client, encoding);

                return encoding.GetString(await message.Content.ReadAsByteArrayAsync()).FromJson<T>();
            }
            catch (Exception ex)
            {
                return CreateDefault<T>(ex);
            }
        }

        static T CreateDefault<T>(Exception ex) where T : CafeBazaarResultBase, new()
        {
            return new T
            {
                Error = "Unhandled Exception",
                ErrorDescription = ex.Message
            };
        }

        static HttpClient CreateClient(string baseAddress)
        {
            return new HttpClient
            {
                BaseAddress = new Uri(baseAddress),
                Timeout = 30.Seconds()
            };
        }
    }
}
