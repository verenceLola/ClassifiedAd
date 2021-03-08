using System;

namespace MarketPlace.Domain.Events
{
    public class ClassifiedAdCreated
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
    }
    public class ClassifiedAdTitleChanged
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
    public class ClassfiedAdTextUpdated
    {
        public Guid Id { get; set; }
        public string AdText { get; set; }
    }
    public class ClassfiedAdPriceUpdated
    {
        public Guid Id { set; get; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
    }
    public class ClassifiedAdSentForReview
    {
        public Guid Id { get; set; }
    }
}
