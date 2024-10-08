using Flatscha.Base.API.ExampleAPI.Client.Clients;
using Flatscha.Base.API.ExampleAPI.Contracts.Services;
using Flatscha.Base.API.Client.Extensions;
using Microsoft.Extensions.DependencyInjection;
using static Flatscha.Base.API.ExampleAPI.Contracts.Constants.RouteConstants;

namespace Flatscha.Base.API.ExampleAPI.Client.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection AddExampleApiClient(this IServiceCollection services, Func<IServiceProvider, Uri> func)
        {
            var baseUri = func.Invoke(services.BuildServiceProvider()).Append(ROOT);

            services.AddHttpClient<ExampleClient>(baseUri.Append(Example.ROOT));
            services.AddHttpClient<UserClient>(baseUri.Append(User.ROOT));

            services.AddTransient<IExampleService, ExampleClient>();
            services.AddTransient<IUserService, UserClient>();

            return services;
        }
    }
}
