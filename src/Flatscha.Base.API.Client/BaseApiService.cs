using Flatscha.Base.API.Client.Extensions;
using Flatscha.Base.API.Contracts.Constants;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Flatscha.Base.API.Client
{
    public abstract class BaseApiService
    {
        protected readonly ILogger<BaseApiService> _logger;
        protected readonly HttpClient _httpClient;

        protected BaseApiService(ILogger<BaseApiService> logger, IHttpClientFactory httpClientFactory)
        {
            this._logger = logger;
            this._httpClient = httpClientFactory.CreateClient(this.GetType().FullName);
        }

        protected virtual Task<HttpRequestMessage> HandleCustomRequestHeaders(HttpRequestMessage requestMessage) => Task.FromResult(requestMessage);

        protected async Task<HttpResponseMessage> GetResponse(HttpRequestMessage request, CancellationToken cancellationToken, HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
        {
            var customRequest = await this.HandleCustomRequestHeaders(request);

            var response = await this._httpClient.SendAsync(customRequest, httpCompletionOption, cancellationToken);

            await response.ThrowIfNotSuccessful();

            return response;
        }

        protected Task Call(HttpRequestMessage request, CancellationToken cancellationToken) => this.GetResponse(request, cancellationToken);

        protected Task Call(string url, HttpMethod method, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(method, url);

            return this.Call(request, cancellationToken);
        }

        protected Task Call(string url, HttpMethod method, object data, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(method, url)
            {
                Content = JsonContent.Create(data)
            };

            return this.Call(request, cancellationToken);
        }


        protected async Task<T> Call<T>(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await this.GetResponse(request, cancellationToken);

            return await response.ReadJsonAsync<T>();
        }

        protected Task<T> Call<T>(string url, HttpMethod method, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(method, url);

            return this.Call<T>(request, cancellationToken);
        }

        protected Task<T> Call<T>(string url, HttpMethod method, object data, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(method, url)
            {
                Content = JsonContent.Create(data)
            };

            return this.Call<T>(request, cancellationToken);
        }


        protected async IAsyncEnumerable<T> CallEnumerable<T>(HttpRequestMessage request, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var response = await this.GetResponse(request, cancellationToken, HttpCompletionOption.ResponseHeadersRead);

            using var stream = await response.Content.ReadAsStreamAsync();

            await foreach (var item in JsonSerializer.DeserializeAsyncEnumerable<T>(stream, SerializationConstants.JsonOptions, cancellationToken: cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return item;
            };
        }

        protected IAsyncEnumerable<T> CallEnumerable<T>(string url, HttpMethod method, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(method, url);

            return this.CallEnumerable<T>(request, cancellationToken);
        }

        protected IAsyncEnumerable<T> CallEnumerable<T>(string url, HttpMethod method, object data, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(method, url)
            {
                Content = JsonContent.Create(data)
            };

            return this.CallEnumerable<T>(request, cancellationToken);
        }


        protected Task Get(string url, CancellationToken cancellationToken = default)
            => this.Call(url, HttpMethod.Get, cancellationToken);

        protected Task<T> Get<T>(string url, CancellationToken cancellationToken = default)
            => this.Call<T>(url, HttpMethod.Get, cancellationToken);


        protected IAsyncEnumerable<T> GetEnumerable<T>(string url, CancellationToken cancellationToken = default)
            => this.CallEnumerable<T>(url, HttpMethod.Get, cancellationToken);



        protected Task Post(string url, CancellationToken cancellationToken = default)
            => this.Call(url, HttpMethod.Post, cancellationToken);

        protected Task Post(string url, object data, CancellationToken cancellationToken = default)
            => this.Call(url, HttpMethod.Post, data, cancellationToken);

        protected Task<T> Post<T>(string url, CancellationToken cancellationToken = default)
            => this.Call<T>(url, HttpMethod.Post, cancellationToken);

        protected Task<T> Post<T>(string url, object data, CancellationToken cancellationToken = default)
            => this.Call<T>(url, HttpMethod.Post, data, cancellationToken);


        protected IAsyncEnumerable<T> PostEnumerable<T>(string url, CancellationToken cancellationToken = default)
             => this.CallEnumerable<T>(url, HttpMethod.Post, cancellationToken);

        protected IAsyncEnumerable<T> PostEnumerable<T>(string url, object data, CancellationToken cancellationToken = default)
            => this.CallEnumerable<T>(url, HttpMethod.Post, data, cancellationToken);



        protected Task Delete(string url, CancellationToken cancellationToken = default)
            => this.Call(url, HttpMethod.Delete, cancellationToken);

        protected Task Delete(string url, object data, CancellationToken cancellationToken = default)
            => this.Call(url, HttpMethod.Delete, data, cancellationToken);

        protected Task<T> Delete<T>(string url, CancellationToken cancellationToken = default)
            => this.Call<T>(url, HttpMethod.Delete, cancellationToken);

        protected Task<T> Delete<T>(string url, object data, CancellationToken cancellationToken = default)
            => this.Call<T>(url, HttpMethod.Delete, data, cancellationToken);


        protected Task Put(string url, CancellationToken cancellationToken = default)
            => this.Call(url, HttpMethod.Put, cancellationToken);
        
        protected Task Put(string url, object data, CancellationToken cancellationToken = default)
            => this.Call(url, HttpMethod.Put, data, cancellationToken);

        protected Task<T> Put<T>(string url, CancellationToken cancellationToken = default)
            => this.Call<T>(url, HttpMethod.Put, cancellationToken);

        protected Task<T> Put<T>(string url, object data, CancellationToken cancellationToken = default)
            => this.Call<T>(url, HttpMethod.Put, data, cancellationToken);
    }
}
