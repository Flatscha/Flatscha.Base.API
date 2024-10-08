using System.Text.Json;

namespace Flatscha.Base.API.Contracts.Constants
{
    public class SerializationConstants
    {
        public static JsonSerializerOptions JsonOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }
}
