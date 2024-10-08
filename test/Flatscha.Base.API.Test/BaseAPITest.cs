using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Collections;
using Flatscha.Base.API.Test.Models;

namespace Flatscha.Base.API.Test
{
    public abstract class BaseAPITest<TIApiService, TProgram> where TIApiService : class where TProgram : class
    {
        private const string _baseAddress = "http://test";

        private IServiceProvider _serviceProvider;
        private Dictionary<string, Dictionary<string, OpenAPIMethod>> _openApiResult;

        private Fixture _fixture = new();

        private Mock<IHttpClientFactory> _mockClientFactory = new();

        private MockHttpMessageHandler _mockHttpMessageHandler = new();

        public BaseAPITest()
        {
            Task.Run(this.Initialize).Wait();
        }

        private async Task Initialize()
        {
            using var app = new WebApplicationFactory<TProgram>();

            using var client = app.CreateClient();

            var jsonString = await client.GetStringAsync("swagger/v1/swagger.json");
            var parsedObject = JObject.Parse(jsonString);

            this._openApiResult = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, OpenAPIMethod>>>(parsedObject["paths"]?.ToString() ?? string.Empty) ?? throw new Exception("Failed to parse paths");

            var services = new ServiceCollection();

            this.AddClientStuff(services, new(_baseAddress));

            var clientFactory = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();

            services.RemoveAll<IHttpClientFactory>();
            services.AddSingleton(this._mockClientFactory.Object);

            this._serviceProvider = services.BuildServiceProvider();

            this._mockClientFactory
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns((string name) =>
                {
                    var client = clientFactory.CreateClient(name);

                    return new HttpClient(this._mockHttpMessageHandler)
                    {
                        BaseAddress = client.BaseAddress
                    };
                });
        }

        protected abstract IServiceCollection AddClientStuff(IServiceCollection services, Uri uri);

        public abstract Task CheckIfAllClientCallsExist();

        protected async Task CheckClientCalls()
        {
            var service = this._serviceProvider.GetRequiredService<TIApiService>();

            foreach (var property in typeof(TIApiService).GetProperties())
            {
                var client = property.GetValue(service);

                if (client is null) { continue; }

                foreach (var method in property.PropertyType.GetMethods())
                {
                    await ExecuteMethodWithMockedParameters(client, method);

                    var request = this._mockHttpMessageHandler.LastRequest ?? throw new Exception($"Method [{method.Name}] did not send a request");

                    var realRequestUrl = this.GetRealCall(request.RequestUri?.AbsoluteUri);

                    if (string.IsNullOrWhiteSpace(realRequestUrl)) { throw new Exception($"Method [{method.Name}] send an empty request [{request.RequestUri?.AbsoluteUri}]"); }

                    var matches = this.GetMatchingRequests(realRequestUrl);

                    if (!matches.Any()) { throw new Exception($"Could not find any matching requests for [{realRequestUrl}]"); }

                    var methodMatches = matches.Where(x => x.Value.Keys.Any(o => o.Equals(request.Method.Method, StringComparison.CurrentCultureIgnoreCase)));

                    if (methodMatches.Count() != 1) { throw new Exception($"Found {methodMatches.Count()} matching [{request.Method.Method}] requests for [{realRequestUrl}]"); }

                    //do not check type due to wrong json serialziation
                    //var match = methodMatches.SingleOrDefault();
                    //var openApiMethod = match.Value.FirstOrDefault(x => x.Key.Equals(request.Method.Method, StringComparison.CurrentCultureIgnoreCase)).Value;

                    //var responseSchema = openApiMethod.Responses.FirstOrDefault(x => x.Key == "200").Value?.Content?.FirstOrDefault().Value?.FirstOrDefault().Value;

                    //this.HandleReturnType(method, responseSchema);
                }
            }
        }

        private async Task ExecuteMethodWithMockedParameters(object? client, MethodInfo method)
        {
            List<object?> parameters = [];
            foreach (var parameter in method.GetParameters())
            {
                object? par;
                try
                {
                    par = this._fixture.Create(parameter.ParameterType, new SpecimenContext(this._fixture));
                }
                catch
                {
                    par = null;
                }

                parameters.Add(par);
            }

            try
            {
                this._mockHttpMessageHandler.LastRequest = null;

                if (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>))
                {
                    var val = method.Invoke(client, [.. parameters]);

                    var toListAsyncMethod = typeof(AsyncEnumerable).GetMethod(nameof(AsyncEnumerable.ToListAsync));
                    toListAsyncMethod = toListAsyncMethod.MakeGenericMethod(method.ReturnType.GenericTypeArguments);
                    toListAsyncMethod.Invoke(val, [val, default(CancellationToken)]);
                }
                else
                {
                    method.Invoke(client, [.. parameters]);
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }

        private void HandleReturnType(MethodInfo method, OpenAPISchema? responseSchema)
        {
            if (method.ReturnType == typeof(Task))
            {
                if (responseSchema is not null) {; }
            }
            else if (method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                this.CheckIfReturnTypeIsValid(method, responseSchema);
            }
            else if (method.ReturnType.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>))
            {
                this.CheckIfReturnTypeIsValid(method, responseSchema);
            }
            else { throw new Exception($"Could not find handling for return type [{method.ReturnType.Name}]"); }
        }

        private void CheckIfReturnTypeIsValid(MethodInfo method, OpenAPISchema? responseSchema)
        {
            if (responseSchema is null) { throw new Exception($"Method [{method.Name}] has no response schema"); }

            var retVal = method.ReturnType.GenericTypeArguments[0];

            if (method.ReturnType.GenericTypeArguments[0].IsAssignableTo(typeof(IEnumerable)) && responseSchema.Type == "array")
            {
                this.ThrowIfValueIsWrongValue(method, retVal.GenericTypeArguments[0], responseSchema.Items);

                return;
            }

            this.ThrowIfValueIsWrongValue(method, retVal, responseSchema);
        }

        private void ThrowIfValueIsWrongValue(MethodInfo method, Type type, OpenAPIItem item)
        {
            var typeName = item.Format switch
            {
                "uuid" => "Guid",
                _ => string.IsNullOrWhiteSpace(item.Reference) ? item.Type : item.Reference.Split('/').LastOrDefault()
            };

            if (!type.Name.Equals(typeName, StringComparison.CurrentCultureIgnoreCase)) { throw new Exception($"Method [{method.Name}] returns [{typeName}] not [{type.Name}] "); }
        }

        protected string? GetRealCall(string? call)
        {
            var res = call?.Replace(_baseAddress, string.Empty);

            if (string.IsNullOrWhiteSpace(res)) { return null; }

            return res.Split('?')[0];
        }

        protected Dictionary<string, Dictionary<string, OpenAPIMethod>> GetMatchingRequests(string request)
        {
            var requestSplit = request.Split('/').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            return this._openApiResult.Where(x =>
            {
                var pathSplit = x.Key.Split('/').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                if (pathSplit.Length != requestSplit.Length) { return false; }

                for (var i = 0; i < pathSplit.Length; i++)
                {
                    if (pathSplit[i].StartsWith('{') && pathSplit[i].EndsWith('}')) { continue; }

                    if (pathSplit[i] != requestSplit[i]) { return false; }
                }

                return true;
            }).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
