using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Flatscha.Base.API.ExampleAPI.Contracts.Constants;

namespace Flatscha.Base.API.ExampleAPI.Endpoints
{
    public static class ComposeEndpoints
    {
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup(RouteConstants.ROOT);

            group.MapExampleEndpoint();
            group.MapUserEndpoint();

            return app;
        }
    }
}
