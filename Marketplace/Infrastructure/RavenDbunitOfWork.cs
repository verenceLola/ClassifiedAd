using System.Threading.Tasks;
using MarketPlace.Framework;
using Raven.Client.Documents.Session;

namespace MarketPlace.Infrastructure
{
    public class RavenDbUnitOfWork : IUnitOfWork
    {
        private readonly IAsyncDocumentSession _session;
        public RavenDbUnitOfWork(IAsyncDocumentSession session) => _session = session;
        public Task Commit() => _session.SaveChangesAsync();
    }
}
