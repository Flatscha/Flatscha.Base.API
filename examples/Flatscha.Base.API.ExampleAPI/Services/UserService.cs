using Flatscha.Base.API.Contracts.Exceptions;
using Flatscha.Base.API.ExampleAPI.Contracts.Dtos;
using Flatscha.Base.API.ExampleAPI.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Flatscha.Base.API.ExampleAPI.Services
{
    public class UserService : IUserService
    {
        private CacheService _cacheService;

        public UserService(CacheService cacheService)
        {
            this._cacheService = cacheService;
        }

        public Task<UserDto> Get(Guid id, CancellationToken cancellationToken = default) => Task.Run(()
            => this._cacheService.Users.FirstOrDefault(x => x.ID == id) ?? throw new NotFoundException<UserDto>(), cancellationToken);

        public async IAsyncEnumerable<UserDto> GetAll([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var user in this._cacheService.Users)
            {
                await Task.Delay(1000, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                yield return user;
            }
        }

        public Task<Guid> Create(CreateUserDto dto, CancellationToken cancellationToken = default) => Task.Run(() =>
        {
            var user = new UserDto
            {
                Name = dto.Name,
                Birthdate = dto.Birthdate,
            };

            this._cacheService.Users.Add(user);

            return user.ID;
        }, cancellationToken);

        public async Task Update(Guid id, CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var user = await this.Get(id, cancellationToken);

            user.Name = dto.Name;
            user.Birthdate = dto.Birthdate;
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await this.Get(id, cancellationToken);

            var removed = this._cacheService.Users.Remove(user);

            if (!removed) { throw new Exception("Error while removing User"); }
        }
    }
}
