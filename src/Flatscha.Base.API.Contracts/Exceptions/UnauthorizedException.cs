using System.Net;

namespace Flatscha.Base.API.Contracts.Exceptions
{
    public class UnauthorizedException : HTTPStatusCodeException
    {
        public UnauthorizedException() : base(HttpStatusCode.Unauthorized)
        {
        }

        public UnauthorizedException(string message) : base(HttpStatusCode.Unauthorized, message)
        {
        }
    }
}
