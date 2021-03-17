using System.Threading.Tasks;
using Raven.Client.Documents.Session;

namespace MarketPlace
{
    public static class Queries
    {
        public static Task<T> Query<T>(
            this IAsyncDocumentSession session,
            QueryModels.GetPublicClassifiedAd query
        )
            => session.LoadAsync<T>(query.ClassifiedAdId.ToString());
    }
}
