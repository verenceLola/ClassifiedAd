using System.Threading.Tasks;

namespace Marketplace.Api.ApplicationServices.Interfaces
{
    public interface IApplicationService
    {
        Task Handle(object command);
    }
}
