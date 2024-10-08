using Microsoft.Extensions.Configuration;

namespace Flatscha.Base.API.Client.Extensions
{
    public static class ConfigurationExtensions

    {
        private const string ApiKeySection = "ApiKeys";
        public static Uri GetApiKey(this IConfiguration configuration, string name) => new Uri(configuration?.GetSection(ApiKeySection)[name]);
    }
}
