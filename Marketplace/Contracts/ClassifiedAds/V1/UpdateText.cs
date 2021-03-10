using System;

namespace Marketplace.Contracts.ClassifiedAds.V1
{
    public class UpdateText
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}
