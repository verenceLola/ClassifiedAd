using System;


namespace MarketPlace.Contracts.ClassifiedAds.V1
{
    public class Create
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
    }
}
