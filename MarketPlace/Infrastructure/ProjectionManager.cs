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
        public ProjectionManager(IEventStoreConnection connection, IProjection[] projections)
        {
            _connection = connection;
            _projections = projections;
        }
        public void Start()
        {
            var settings = new CatchUpSubscriptionSettings(2000, 500, Log.IsEnabled(LogEventLevel.Verbose), false, "try-out-subscription");
            _subscription = _connection.SubscribeToAllFrom(Position.Start, settings, EventAppeared);
        }
        private Task EventAppeared(EventStoreCatchUpSubscription subscription, ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventType.StartsWith("$"))
            {
                return Task.CompletedTask;
            }
            var @event = resolvedEvent.Deserialize();
            Log.Debug("Projecting event {type}", @event.GetType().Name);

            return Task.WhenAll(_projections.Select(x => x.Project(@event)));
        }
        public void Stop() => _subscription.Stop();
    }
}
