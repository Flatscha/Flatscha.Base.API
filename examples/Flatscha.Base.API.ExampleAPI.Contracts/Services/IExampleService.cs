using System.Threading;
using System.Threading.Tasks;

namespace Flatscha.Base.API.ExampleAPI.Contracts.Services
{
    public interface IExampleService
    {
        Task<string> Hello(string name, CancellationToken cancellationToken = default);
    }
}
