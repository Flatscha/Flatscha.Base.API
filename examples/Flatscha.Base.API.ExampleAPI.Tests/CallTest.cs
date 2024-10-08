using Flatscha.Base.API.Test;
using Microsoft.Extensions.DependencyInjection;
using Flatscha.Base.API.ExampleAPI.Contracts.Services;
using Flatscha.Base.API.ExampleAPI.Client.Extensions;

namespace Flatscha.Base.API.ExampleAPI.Tests
{
    public class CallTest : BaseAPITest<IExampleService, Program>
    {
        protected override IServiceCollection AddClientStuff(IServiceCollection services, Uri uri)
        {
            services.AddExampleApiClient(sp => uri);

            return services;
        }

        [Fact]
        public override async Task CheckIfAllClientCallsExist() => await this.CheckClientCalls();
    }
}
