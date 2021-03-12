using System;

namespace Marketplace.ReadModels.ClassifiedAds
{
    public class PublicClassifiedAdListItem
    {
        public Guid ClassifiedAdId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
        public string PhotoUrl { get; set; }
    }
}
