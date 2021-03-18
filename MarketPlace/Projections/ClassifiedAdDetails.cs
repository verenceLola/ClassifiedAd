using System;
using System.Linq;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;
using System.Collections.Generic;
using Raven.Client.Documents.Session;
using MarketPlace.Domain.Events.ClassifiedAdEvents;
using MarketPlace.Domain.Events.UserProfile;

namespace MarketPlace.Projections
{
    public class ClassifiedAdDetailProjection : RavenDBProjection<ReadModels.ClassifiedAds.ClassfiedAdDetails>
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ClassifiedAdDetailProjection>();
        private Func<Guid, Task<string>> _getUserDisplayName;
        public ClassifiedAdDetailProjection(Func<IAsyncDocumentSession> getSession, Func<Guid, Task<string>> getUserDisplayName) : base(getSession)
        {
            _getUserDisplayName = getUserDisplayName;
        }

        public override Task Project(object @event) =>
        @event switch
        {
            ClassifiedAdCreated e =>
                Create(async () => new ReadModels.ClassifiedAds.ClassfiedAdDetails
                {
                    ClassifiedAdId = e.Id,
                    SellerId = e.OwnerId,
                    SellerDisplayName = await _getUserDisplayName(e.OwnerId),
                    Id = e.Id.ToString()
                }),
            ClassifiedAdTitleChanged e =>
                UpdateOne(e.Id, ad => ad.Title = e.Title),
            ClassfiedAdTextUpdated e =>
                UpdateOne(e.Id, ad => ad.Description = e.AdText),
            ClassfiedAdPriceUpdated e =>
                UpdateOne(e.Id, ad =>
                {
                    ad.Price = e.Price;
                    ad.CurrencyCode = e.CurrencyCode;
                }),
            UserDisplayNameUpdated e =>
                UpdateWhere(x => x.SellerId == e.UserId, x => x.SellerDisplayName = e.DisplayName),
            Projections.ClassifiedAdUpcasters.ClassifiedAdUpcastedEvents.V1.ClassifiedAdPublished e =>
                UpdateOne(e.Id, ad => ad.SellerPhotoUrl = e.SellerPhotoUrl),
            _ => Task.CompletedTask,
        };
    }
}
