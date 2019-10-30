using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DA.HttpClients
{
    public static class HttpRequestClient
    {
        private static readonly HttpClient Client = new HttpClient();

        public static async Task<T> GetHttpContent<T>(string uri, Func<string, T> handler)
        {
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 YaBrowser/19.10.1.238 Yowser/2.5 Safari/537.36");
            string resultHtml;
            using (HttpResponseMessage response = await Client.GetAsync(uri))
            using (HttpContent content = response.Content)
            {
                resultHtml = await content.ReadAsStringAsync();
            }
            return handler(resultHtml);
        }
    }
}