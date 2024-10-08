using Flatscha.Base.API.Client;
using Flatscha.Base.API.ExampleAPI.Contracts.Extensions;
using Flatscha.Base.API.ExampleAPI.Contracts.Dtos;
using Flatscha.Base.API.ExampleAPI.Contracts.Services;
using Microsoft.Extensions.Logging;
using static Flatscha.Base.API.ExampleAPI.Contracts.Constants.RouteConstants.User;

namespace Flatscha.Base.API.ExampleAPI.Client.Clients
{
    public class UserClient : BaseApiService, IUserService
    {
        public UserClient(ILogger<ExampleClient> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
        {
        }

        public async Task<Guid> Create(CreateUserDto dto, CancellationToken cancellationToken = default) => await this.Post<Guid>(CREATE, dto, cancellationToken);

        public async Task Delete(Guid id, CancellationToken cancellationToken = default) => await this.Delete(DELETE.SetRouteParameterID(id), cancellationToken);

        public async Task<UserDto> Get(Guid id, CancellationToken cancellationToken = default) => await this.Get<UserDto>(GET.SetRouteParameterID(id), cancellationToken);

        public IAsyncEnumerable<UserDto> GetAll(CancellationToken cancellationToken = default) => this.GetEnumerable<UserDto>(GET_ALL, cancellationToken);

        public async Task Update(Guid id, CreateUserDto dto, CancellationToken cancellationToken = default) => await this.Post(UPDATE.SetRouteParameterID(id), dto, cancellationToken);
    }
}
