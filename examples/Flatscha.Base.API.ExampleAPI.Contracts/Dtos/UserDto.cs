using System;

namespace Flatscha.Base.API.ExampleAPI.Contracts.Dtos
{
    public class UserDto
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public DateTime? Birthdate { get; set; } = null;
    }
}
