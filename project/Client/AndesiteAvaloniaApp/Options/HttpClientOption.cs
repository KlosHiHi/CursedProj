using System;
using System.Net.Http;

namespace AndesiteAvaloniaApp.Options
{
    public class HttpClientOption
    {
        public static HttpClient Client { get; }
        private readonly static string _baseUrl = "https://localhost:7122/api/";
        //private readonly static string _baseUrl = "https://localhost:2505/api/";

        static HttpClientOption()
        {
            var handler = new BearerTokenHandler
            {
                InnerHandler = new HttpClientHandler()
            };

            Client = new(handler)
            {
                BaseAddress = new Uri(_baseUrl),
                Timeout = TimeSpan.FromSeconds(30),
            };
        }
    }
}
