using Flatscha.Base.API.ExampleAPI.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Flatscha.Base.API.ExampleAPI.Contracts.Services
{
    public interface IUserService
    {
        Task<Guid> Create(CreateUserDto dto, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task<UserDto> Get(Guid id, CancellationToken cancellationToken = default);
        IAsyncEnumerable<UserDto> GetAll(CancellationToken cancellationToken = default);
        Task Update(Guid id, CreateUserDto dto, CancellationToken cancellationToken = default);
    }
}
