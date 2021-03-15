using System;

namespace MarketPlace.ReadModels.ClassifiedAds
{
    public class ClassfiedAdDetails
    {
        public Guid ClassifiedAdId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public string SellerDisplayName { get; set; }
        public string[] PhotoUrls { get; set; }
        public Guid SellerId { get; set; }
    }
}
