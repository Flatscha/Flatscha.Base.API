using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using System.Threading;
using Flatscha.Base.API.ExampleAPI.Contracts.Services;
using static Flatscha.Base.API.ExampleAPI.Contracts.Constants.RouteConstants.Example;

namespace Flatscha.Base.API.ExampleAPI.Endpoints
{
    public static class ExampleEndpoint
    {
        public static IEndpointRouteBuilder MapExampleEndpoint(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup(ROOT).WithTags(ROOT);

            group.MapGet(HELLO, Hello);

            return app;
        }

        private static Task<string> Hello(string name, IExampleService service, CancellationToken cancellationToken = default) => service.Hello(name, cancellationToken);
    }
}
