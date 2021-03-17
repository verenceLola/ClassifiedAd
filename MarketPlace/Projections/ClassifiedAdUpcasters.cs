using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using MarketPlace.Framework;
using MarketPlace.Domain.Events.ClassifiedAdEvents;

namespace MarketPlace.Projections
{
    public class ClassifiedAdUpcasters : IProjection
    {
        private readonly IEventStoreConnection _connection;
        private readonly Func<Guid, Task<String>> _getUserPhoto;
        private const string streamName = "UpcastedClassifiedAdEvents";

        public ClassifiedAdUpcasters(IEventStoreConnection connection, Func<Guid, Task<string>> getUserPhoto)
        {
            _connection = connection;
            _getUserPhoto = getUserPhoto;
        }
        public async Task Project(object @event)
        {
            switch (@event)
            {
                case ClassifiedAdPublished e:
                    var photoUrl = await _getUserPhoto(e.OwnerId);
                    var newEvent = new ClassifiedAdUpcastedEvents.V1.ClassifiedAdPublished
                    {
                        Id = e.Id,
                        OwnerId = e.OwnerId,
                        ApprovedBy = e.ApprovedBy,
                        SellerPhotoUrl = photoUrl
                    };
                    await _connection.AppendEvent(streamName, ExpectedVersion.Any, newEvent);
                    break;
            }
        }
        public static class ClassifiedAdUpcastedEvents
        {
            public static class V1
            {
                public class ClassifiedAdPublished
                {
                    public Guid Id { get; set; }
                    public Guid OwnerId { get; set; }
                    public string SellerPhotoUrl { get; set; }
                    public Guid ApprovedBy { get; set; }
                }
            }
        }
    }
}
