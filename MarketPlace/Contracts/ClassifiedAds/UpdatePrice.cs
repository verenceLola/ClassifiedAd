using System;

namespace MarketPlace.Contracts.ClassifiedAds
{
    public class UpdatePrice
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
