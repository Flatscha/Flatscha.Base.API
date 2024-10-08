using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using System.Threading;
using Flatscha.Base.API.ExampleAPI.Contracts.Services;
using static Flatscha.Base.API.ExampleAPI.Contracts.Constants.RouteConstants.User;
using Flatscha.Base.API.ExampleAPI.Contracts.Dtos;
using System;
using System.Collections.Generic;

namespace Flatscha.Base.API.ExampleAPI.Endpoints
{
    public static class UserEndpoint
    {
        public static IEndpointRouteBuilder MapUserEndpoint(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup(ROOT).WithTags(ROOT);

            group.MapPut(CREATE, Create);
            group.MapDelete(DELETE, Delete);
            group.MapGet(GET, Get);
            group.MapGet(GET_ALL, GetAll);
            group.MapPost(UPDATE, Update);

            return app;
        }

        private static Task<Guid> Create(CreateUserDto dto, IUserService service, CancellationToken cancellationToken = default) => service.Create(dto, cancellationToken);
        private static Task Delete(Guid id, IUserService service, CancellationToken cancellationToken = default) => service.Delete(id, cancellationToken);
        private static Task<UserDto> Get(Guid id, IUserService service, CancellationToken cancellationToken = default) => service.Get(id, cancellationToken);
        private static IAsyncEnumerable<UserDto> GetAll(IUserService service, CancellationToken cancellationToken = default) => service.GetAll(cancellationToken);
        private static Task Update(Guid id, CreateUserDto dto, IUserService service, CancellationToken cancellationToken = default) => service.Update(id, dto, cancellationToken);
    }
}
