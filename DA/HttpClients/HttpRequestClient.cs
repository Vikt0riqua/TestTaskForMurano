using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DA.HttpClients
{
    public static class HttpRequestClient
    {
        private static readonly HttpClient Client = new HttpClient();
        
        public static async Task<T> GetHttpContent <T>(string uri, Func<string,T> handler)
        {
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