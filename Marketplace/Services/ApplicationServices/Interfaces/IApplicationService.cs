using System.Threading.Tasks;

namespace MarketPlace.Services.ApplicationServices.Interfaces
{
    public interface IApplicationService
    {
        Task Handle(object command);
    }
}
