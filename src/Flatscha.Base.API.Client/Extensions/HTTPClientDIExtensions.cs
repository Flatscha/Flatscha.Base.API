using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Flatscha.Base.API.Client.Extensions
{
    public static class HttpClientDIExtensions
    {
        public static HttpClient CreateClient<TService>(this IHttpClientFactory httpClientFactory) where TService : class
            => httpClientFactory.CreateClient(typeof(TService).Name);

        public static IServiceCollection AddHttpClient(this IServiceCollection services, string name, Func<IServiceProvider, Uri> func)
            => services.AddHttpClient(name, func.Invoke(services.BuildServiceProvider()));

        public static IServiceCollection AddHttpClient<TService>(this IServiceCollection services, Func<IServiceProvider, Uri> func) where TService : class
            => services.AddHttpClient<TService>(func.Invoke(services.BuildServiceProvider()));

        public static IServiceCollection AddHttpClient<TService>(this IServiceCollection services, Uri uri) where TService : class
            => services.AddHttpClient(typeof(TService).FullName, uri);

        private static IServiceCollection AddHttpClient(this IServiceCollection services, string name, Uri baseAddress)
        {
            services.AddHttpClient(name, client =>
            {
                client.BaseAddress = baseAddress.Append("/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            return services;
        }
    }
}
