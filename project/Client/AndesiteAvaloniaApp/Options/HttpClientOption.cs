using System.Net.Http;

namespace AndesiteAvaloniaApp.Options
{
    public class HttpClientOption
    {
        static HttpClientOption()
        {
            Client.DefaultRequestHeaders.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "token");
        }

        public static HttpClient Client { get; private set; } = new();
        public static string BaseUrl { get; private set; } = "https://localhost:7039/api/";
    }
}
