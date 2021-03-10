using MarketPlace.Domain.Interfaces;
using MarketPlace.Domain.Entities;
using MarketPlace.Domain.ValueObjects.ClasifiedAd;
using MarketPlace.Framework;
using Raven.Client.Documents.Session;

namespace Marketplace.Infrastructure
{
    public class ClassifiedAdRepository : RavenDBRepository<ClassfiedAd, ClassfiedAdId>, IClassifiedAdRepository
    {

        public ClassifiedAdRepository(IAsyncDocumentSession session) : base(session, id => $"ClassifiedAd/{id.Value.ToString()}") { }
    }
}
