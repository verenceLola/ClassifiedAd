using System.Linq;
using System.Collections.Generic;
using Raven.Client.Documents;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;
using MarketPlace.Domain.Entities;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Linq;

namespace MarketPlace
{
    public static class Queries
    {
        public static ReadModels.ClassifiedAds.ClassfiedAdDetails Query(this IEnumerable<ReadModels.ClassifiedAds.ClassfiedAdDetails> items, QueryModels.GetPublicClassifiedAd query)
            => items.FirstOrDefault(x => x.ClassifiedAdId == query.ClassifiedAdId);
    }
}
