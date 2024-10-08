using System;
using System.Net;

namespace Flatscha.Base.API.Contracts.Exceptions
{
    public class HTTPStatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public HTTPStatusCodeException(HttpStatusCode statusCode) : this(statusCode, statusCode.ToString())
        {
        }

        public HTTPStatusCodeException(HttpStatusCode statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
