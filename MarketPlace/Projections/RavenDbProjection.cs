using System;
using Raven.Client.Documents.Session;
using MarketPlace.Framework;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents;

namespace MarketPlace.Projections
{
    public abstract class RavenDBProjection<T> : IProjection
    {
        protected RavenDBProjection(Func<IAsyncDocumentSession> getSession) => GetSession = getSession;
        protected Func<IAsyncDocumentSession> GetSession { get; }
        public abstract Task Project(object @event);
        protected Task Create(Func<Task<T>> model) =>
            UsingSession(async session => await session.StoreAsync(await model()));
        protected Task UpdateOne(Guid id, Action<T> update) =>
            UsingSession(session => UpdateItem(session, id, update));
        protected Task UpdateWhere(Expression<Func<T, bool>> where, Action<T> update) =>
            UsingSession(session => UpdateMultipleItems(session, where, update));
        protected async Task UsingSession(Func<IAsyncDocumentSession, Task> operation)
        {
            using var session = GetSession();

            await operation(session);
            await session.SaveChangesAsync();
        }
        private static async Task UpdateItem(IAsyncDocumentSession session, Guid id, Action<T> update)
        {
            var item = await session.LoadAsync<T>(id.ToString());
            if (item == null) return;

            update(item);
        }
        private static async Task UpdateMultipleItems(IAsyncDocumentSession session, Expression<Func<T, bool>> query, Action<T> update)
        {
            var items = await session.Query<T>().Where(query).ToListAsync();

            foreach (var item in items)
            {
                update(item);
            }
        }
    }
}
