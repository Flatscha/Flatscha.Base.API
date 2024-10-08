using Flatscha.Base.API.Client.Exceptions;
using Flatscha.Base.API.Contracts.Constants;
using System.Net;
using System.Text.Json;

namespace Flatscha.Base.API.Client.Extensions
{
    public static class HTTPResponseExtensions
    {
        private static async Task<string> GetErrorMessage(this HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var message = responseContent?.Trim('"');

            if (!string.IsNullOrWhiteSpace(message)) { return message; }

            return $"{(int)response.StatusCode} {response.StatusCode}";
        }

        public static async Task ThrowIfNotSuccessful(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) { return; }

            if (response.StatusCode == HttpStatusCode.ServiceUnavailable) { throw new TimeoutException("Konnte API nicht erreichen"); }

            var errorMessage = await response.GetErrorMessage();

            throw new APIException(errorMessage, response);
        }

        public static async Task<T> ReadJsonAsync<T>(this HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStringAsync();

            if (typeof(T) == typeof(string) && response.Content.Headers.ContentType?.MediaType == "text/plain") { return (T)Convert.ChangeType(data, typeof(T)); }

            return JsonSerializer.Deserialize<T>(data, SerializationConstants.JsonOptions);
        }
    }
}
