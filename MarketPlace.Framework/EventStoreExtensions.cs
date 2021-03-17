using System;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System.Text;

namespace MarketPlace.Framework
{
    public static class EventStoreExtensions
    {
        public static Task AppendEvent(
            this IEventStoreConnection connection,
            string streamName, long version,
            params object[] events
        )
        {
            if (events == null || !events.Any()) return Task.CompletedTask;
            var preparedEvents = events
                .Select(@event => new EventData(
                    eventId: Guid.NewGuid(),
                    type: @event.GetType().Name,
                    isJson: true,
                    data: Serialize(@event),
                    metadata: Serialize(
                        new EventMetadata
                        {
                            ClrType = @event.GetType().AssemblyQualifiedName
                        }
                    )
                ))
                .ToArray();

            return connection.AppendToStreamAsync(streamName, version, preparedEvents);
        }
        private static byte[] Serialize(object data) =>
            Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        private class EventMetadata
        {
            public string ClrType { get; set; }
        }
    }
}
