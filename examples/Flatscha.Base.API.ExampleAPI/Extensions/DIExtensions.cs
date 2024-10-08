using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Flatscha.Base.API.ExampleAPI.Services;
using Flatscha.Base.API.ExampleAPI.Contracts.Services;

namespace Flatscha.Base.API.ExampleAPI.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection AddExampleApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<CacheService>();

            services.AddTransient<IExampleService, ExampleService>();
            services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
