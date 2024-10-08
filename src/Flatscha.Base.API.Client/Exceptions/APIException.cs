using System.Net;

namespace Flatscha.Base.API.Client.Exceptions
{
    public class APIException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public APIException(string message, HttpResponseMessage response) : base(message)
        {
            this.StatusCode = response.StatusCode;
        }
    }
}
