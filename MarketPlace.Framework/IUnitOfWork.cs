using System.Threading.Tasks;

namespace MarketPlace.Framework
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
