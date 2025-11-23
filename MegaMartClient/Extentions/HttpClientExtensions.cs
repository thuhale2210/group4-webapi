using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MegaMartClient.Extensions
{
    // Implement Patch Client Side
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUri)
            {
                Content = JsonContent.Create(value)
            };

            return client.SendAsync(request);
        }
    }
}
