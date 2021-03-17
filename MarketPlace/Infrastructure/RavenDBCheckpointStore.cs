using System;
using MarketPlace.Framework;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;
using EventStore.ClientAPI;


namespace MarketPlace.Infrastructure
{
    public class RavenDBCheckpointStore : ICheckpointStore
    {
        private readonly Func<IAsyncDocumentSession> _getSession;
        private readonly string _checkpointName;
        public RavenDBCheckpointStore(Func<IAsyncDocumentSession> getSession, string checkpointName)
        {
            _getSession = getSession;
            _checkpointName = checkpointName;
        }
        public async Task<Position> GetCheckpoint()
        {
            using var session = _getSession();
            var checkpoint = await session.LoadAsync<Checkpoint>(_checkpointName);

            return checkpoint?.Position ?? Position.Start;
        }
        public async Task StoreCheckpoint(Position position)
        {
            using var session = _getSession();
            var checkpoint = await session.LoadAsync<Checkpoint>(_checkpointName);

            if (checkpoint == null)
            {
                checkpoint = new Checkpoint
                {
                    Id = _checkpointName
                };
                await session.StoreAsync(checkpoint);
            }

            checkpoint.Position = position;
            await session.SaveChangesAsync();
        }
    }
}
