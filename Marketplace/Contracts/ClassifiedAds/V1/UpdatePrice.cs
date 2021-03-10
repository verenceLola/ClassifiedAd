using System;

namespace Marketplace.Contracts.ClassifiedAds.V1
{
    public class UpdatePrice
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
