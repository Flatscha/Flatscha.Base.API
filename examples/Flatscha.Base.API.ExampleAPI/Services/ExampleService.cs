using Flatscha.Base.API.ExampleAPI.Contracts.Services;
using System.Threading.Tasks;
using System.Threading;

namespace Flatscha.Base.API.ExampleAPI.Services
{
    public class ExampleService : IExampleService
    {
        public Task<string> Hello(string name, CancellationToken cancellationToken = default) => Task.Run(() => "Hello " + name, cancellationToken);
    }
}
