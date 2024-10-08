using Flatscha.Base.API.ExampleAPI.Contracts.Dtos;
using System.Collections.Generic;

namespace Flatscha.Base.API.ExampleAPI.Services
{
    public class CacheService
    {
        public List<UserDto> Users { get; set; } = [];
    }
}
