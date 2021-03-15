using System;

namespace MarketPlace.QueryModels
{
    public class GetPublishedClassifiedAds
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
    public class GetOwnersClassifiedAds
    {
        public Guid OwnerId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
    public class GetPublicClassifiedAd
    {
        public Guid ClassifiedAdId { get; set; }
    }
}
