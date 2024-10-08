using Newtonsoft.Json;

namespace Flatscha.Base.API.Test.Models
{
    public class OpenAPIItem
    {
        [JsonProperty("$ref")]
        public string Reference { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }
        public bool Nullable { get; set; }
    }
}
