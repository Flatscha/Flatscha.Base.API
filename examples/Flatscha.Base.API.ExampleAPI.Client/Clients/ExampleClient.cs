using Flatscha.Base.API.Client;
using Flatscha.Base.API.ExampleAPI.Contracts.Services;
using Microsoft.Extensions.Logging;
using Flatscha.Base.API.ExampleAPI.Contracts.Extensions;
using static Flatscha.Base.API.ExampleAPI.Contracts.Constants.RouteConstants.Example;

namespace Flatscha.Base.API.ExampleAPI.Client.Clients
{
    public class ExampleClient : BaseApiService, IExampleService
    {
        public ExampleClient(ILogger<ExampleClient> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
        {
        }

        public async Task<string> Hello(string name, CancellationToken cancellationToken = default) => await this.Get<string>(HELLO.SetRouteParameterName(name), cancellationToken);
    }
}
