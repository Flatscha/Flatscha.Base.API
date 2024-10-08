using System.Net;

namespace Flatscha.Base.API.Contracts.Exceptions
{
    public class NotFoundException : HTTPStatusCodeException
    {
        public string Name { get; } = string.Empty;

        public NotFoundException() : base(HttpStatusCode.NotFound)
        {
        }

        public NotFoundException(string name) : base(HttpStatusCode.NotFound, $"{name} not found")
        {
            this.Name = name;
        }
    }

    public class NotFoundException<T> : NotFoundException
    {
        public NotFoundException() : base(typeof(T).Name)
        {
        }
    }
}
