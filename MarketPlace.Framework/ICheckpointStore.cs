using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace MarketPlace.Framework
{
    public interface ICheckpointStore
    {
        Task<Position> GetCheckpoint();
        Task StoreCheckpoint(Position checkpoint);
    }
    public class Checkpoint
    {
        public string Id { get; set; }
        public Position Position { get; set; }
    }
}
