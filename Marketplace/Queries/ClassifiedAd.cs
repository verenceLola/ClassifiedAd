using System.Threading.Tasks;
using Raven.Client.Documents.Session;

namespace MarketPlace
{
    public static class Queries
    {
        public static Task<ReadModels.ClassifiedAds.ClassfiedAdDetails> Query<T>(
            this IAsyncDocumentSession session,
            QueryModels.GetPublicClassifiedAd query
        )
            => session.LoadAsync<ReadModels.ClassifiedAds.ClassfiedAdDetails>(query.ClassifiedAdId.ToString());
    }
}
