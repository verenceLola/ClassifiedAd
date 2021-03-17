using System;

namespace MarketPlace.Contracts.ClassifiedAds
{
    public class UpdateText
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}
