using System;

namespace Flatscha.Base.API.ExampleAPI.Contracts.Dtos
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public DateTime? Birthdate { get; set; } = null;
    }
}
