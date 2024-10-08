namespace Flatscha.Base.API.Test.Models
{
    public class OpenAPIResponse
    {
        public string Description { get; set; }

        public Dictionary<string, Dictionary<string, OpenAPISchema>> Content { get; set; }
    }
}
