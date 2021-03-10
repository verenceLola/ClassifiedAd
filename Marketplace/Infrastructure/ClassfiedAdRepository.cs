using System.Threading.Tasks;
using MarketPlace.Domain.Interfaces;
using Raven.Client.Documents.Session;
using MarketPlace.Domain.Entities;
using MarketPlace.Domain.ValueObjects;

namespace Marketplace.Infrastructure
{
    public class ClassifiedAdRepository : IClassifiedAdRepository
    {
        private readonly IAsyncDocumentSession _session;
        public ClassifiedAdRepository(IAsyncDocumentSession session) => _session = session;
        public Task<ClassfiedAd> Load(ClassfiedAdId id) => _session.LoadAsync<ClassfiedAd>(EntityId(id));
        public Task Add(ClassfiedAd entity) => _session.StoreAsync(entity, EntityId(entity.Id));
        public Task<bool> Exists(ClassfiedAdId id) => _session.Advanced.ExistsAsync(EntityId(id));
        private static string EntityId(ClassfiedAdId id) => $"ClassifiedAd/{id.ToString()}";
    }
}
