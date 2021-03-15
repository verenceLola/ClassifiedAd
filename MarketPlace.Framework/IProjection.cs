using System.Threading.Tasks;

namespace MarketPlace.Framework
{
    public interface IProjection
    {
        Task Project(object @event);
    }
}
