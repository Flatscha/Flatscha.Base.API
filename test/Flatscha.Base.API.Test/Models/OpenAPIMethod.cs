namespace Flatscha.Base.API.Test.Models
{
    public class OpenAPIMethod
    {
        public List<string> Tags { get; set; }
        public List<OpenAPIParameter> Parameters { get; set; }
        public Dictionary<string, OpenAPIResponse> Responses { get; set; }
    }
}
