using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aggregator.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> ReadAs<T>(this HttpResponseMessage response)  
        {
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"error calling api: {response.ReasonPhrase}");

            var responseStr = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(responseStr);
        }
    }
}
