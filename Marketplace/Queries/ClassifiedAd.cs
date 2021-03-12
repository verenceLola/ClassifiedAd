using System.Linq;
using System.Collections.Generic;
using Raven.Client.Documents;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;
using MarketPlace.Domain.Entities;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Linq;

namespace Marketplace
{
    public static class Queries
    {
        public static Task<List<ReadModels.ClassifiedAds.PublicClassifiedAdListItem>> Query(this IAsyncDocumentSession session, QueryModels.GetPublishedClassifiedAds query) =>
            session.Query<ClassfiedAd>()
                .Where(x => x.State == ClassfiedAd.ClassifiedAdState.Active)
                .Select(x => new ReadModels.ClassifiedAds.PublicClassifiedAdListItem
                {
                    ClassifiedAdId = x.Id.Value,
                    Price = x.Price.Amount,
                    Title = x.Title.Value,
                    CurrencyCode = x.Price.Currency.CurrencyCode
                })
                .PagedList(query.Page, query.PageSize);

        public static Task<List<ReadModels.ClassifiedAds.PublicClassifiedAdListItem>> Query(this IAsyncDocumentSession session, QueryModels.GetOwnersClassifiedAds query)
            => session.Query<ClassfiedAd>()
                .Where(x => x.OwnerId.Value == query.OwnerId)
                .Select(x => new ReadModels.ClassifiedAds.PublicClassifiedAdListItem
                {
                    ClassifiedAdId = x.Id.Value,
                    Price = x.Price.Amount,
                    Title = x.Title.Value,
                    CurrencyCode = x.Price.Currency.CurrencyCode,
                })
                .PagedList(query.Page, query.PageSize);

        public static Task<ReadModels.ClassifiedAds.ClassfiedAdDetails> Query(this IAsyncDocumentSession session, QueryModels.GetPublicClassifiedAd query)
            => (from ad in session.Query<ClassfiedAd>()
                where ad.Id.Value == query.ClassifiedAdId
                let user = RavenQuery.Load<UserProfile>("UserProfile/" + ad.OwnerId.Value)
                select new ReadModels.ClassifiedAds.ClassfiedAdDetails
                {
                    ClassifiedAdId = ad.Id.Value,
                    Title = ad.Title.Value,
                    Description = ad.Text.Value,
                    Price = ad.Price.Amount,
                    CurrencyCode = ad.Price.Currency.CurrencyCode,
                    SellerDisplayName = user.DisplayName.Value
                }).SingleAsync();

        public static Task<List<T>> PagedList<T>(this IRavenQueryable<T> query, int page, int pageSize)
            => query
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }
}
