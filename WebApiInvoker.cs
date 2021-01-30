namespace CafeBazaar.DeveloperApi
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Olive;

    class WebApiInvoker
    {
        public Uri BaseAddress { get; }
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public TimeSpan Timeout { get; set; } = 30.Seconds();

        public WebApiInvoker(Uri baseAddress) => BaseAddress = baseAddress;

        public Task<T> Get<T>(string path) where T : CafeBazaarResultBase, new()
        {
            return Send<T>((client, _) => client.GetAsync(path));
        }

        public Task<T> PostJson<T>(string path, object request) where T : CafeBazaarResultBase, new()
        {
            return Send<T>(async (client, enc) =>
            {
                var payload = new StringContent(request.ToJson(), Encoding, "text/json");
                return await client.PostAsync(path, payload);
            });
        }

        public Task<T> PostForm<T>(string path, object request) where T : CafeBazaarResultBase, new()
        {
            return Send<T>(async (client, enc) =>
            {
                var payload = new FormUrlEncodedContent(request.ToDictionary());
                return await client.PostAsync(path, payload);
            });
        }

        async Task<T> Send<T>(Func<HttpClient, Encoding, Task<HttpResponseMessage>> requestInitiator) where T : CafeBazaarResultBase, new()
        {
            try
            {
                var client = CreateClient();

                var message = await requestInitiator(client, Encoding);

                return Encoding.GetString(await message.Content.ReadAsByteArrayAsync()).FromJson<T>();
            }
            catch (Exception ex)
            {
                return CreateDefault<T>(ex);
            }
        }

        T CreateDefault<T>(Exception ex) where T : CafeBazaarResultBase, new()
        {
            return new T
            {
                Error = "Unhandled Exception",
                ErrorDescription = ex.Message
            };
        }

        HttpClient CreateClient()
        {
            return new HttpClient
            {
                BaseAddress = BaseAddress,
                Timeout = Timeout
            };
        }
    }
}
