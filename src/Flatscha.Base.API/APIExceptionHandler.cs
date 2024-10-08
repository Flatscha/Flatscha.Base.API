using System.Net;
using Flatscha.Base.API.Contracts.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Flatscha.Base.API
{
    public class APIExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<APIExceptionHandler> _logger;

        public APIExceptionHandler(ILogger<APIExceptionHandler> logger)
        {
            this._logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            this._logger?.LogError(exception, $"[{httpContext.Connection.RemoteIpAddress}] failed to execute [{httpContext.Request.Path}]");

            (HttpStatusCode statusCode, string message) = exception switch
            {
                HTTPStatusCodeException ex => (ex.StatusCode, ex.Message),
                _ => (HttpStatusCode.InternalServerError, exception.Message)
            };

            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(message, cancellationToken);

            return true;
        }
    }
}
