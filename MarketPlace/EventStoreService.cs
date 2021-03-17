using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using EventStore.ClientAPI;
using System.Threading;

namespace MarketPlace
{
    public class EventStoreService : IHostedService
    {
        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly Infrastructure.ProjectionManager _projectionManager;
        public EventStoreService(IEventStoreConnection eventStoreConnection, Infrastructure.ProjectionManager projectionManager)
        {
            _eventStoreConnection = eventStoreConnection;
            _projectionManager = projectionManager;
        }
        public  async Task StartAsync(CancellationToken cancellationToken){
            await _eventStoreConnection.ConnectAsync();
            await _projectionManager.Start();
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _projectionManager.Stop();
            _eventStoreConnection.Close();

            return Task.CompletedTask;
        }
    }
}
