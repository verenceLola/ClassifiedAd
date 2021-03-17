using EventStore.ClientAPI;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Events;
using ILogger = Serilog.ILogger;
using MarketPlace.Framework;

namespace MarketPlace.Infrastructure
{
    public class ProjectionManager
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ProjectionManager>();
        private readonly IEventStoreConnection _connection;
        private readonly IProjection[] _projections;
        private EventStoreAllCatchUpSubscription _subscription;
        private readonly ICheckpointStore _checkpointStore;
        public ProjectionManager(IEventStoreConnection connection, ICheckpointStore checkpointStore, params IProjection[] projections)
        {
            _connection = connection;
            _projections = projections;
            _checkpointStore = checkpointStore;
        }
        public async Task Start()
        {
            var settings = new CatchUpSubscriptionSettings(2000, 500, Log.IsEnabled(LogEventLevel.Verbose), false, "try-out-subscription");
            var position = await _checkpointStore.GetCheckpoint();
            _subscription = _connection.SubscribeToAllFrom(position, settings, EventAppeared);
        }
        private async Task EventAppeared(EventStoreCatchUpSubscription _, ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventType.StartsWith("$")) return;
            var @event = resolvedEvent.Deserialize();
            Log.Debug("Projecting event {type}", @event.GetType().Name);

            await Task.WhenAll(_projections.Select(x => x.Project(@event)));
            await _checkpointStore.StoreCheckpoint(resolvedEvent.OriginalPosition.Value);
        }
        public void Stop() => _subscription.Stop();
    }
}
