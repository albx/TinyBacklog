using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace TinyBacklog.Api.Extensions
{
    public static class HttpRequestExtensions
    {
        public static async Task<T> ParseRequestBodyAsync<T>(this HttpRequest request) where T : class
        {
            using var reader = new StreamReader(request.Body);
            string requestBody = await reader.ReadToEndAsync();

            return JsonConvert.DeserializeObject<T>(requestBody);
        }
    }
}
