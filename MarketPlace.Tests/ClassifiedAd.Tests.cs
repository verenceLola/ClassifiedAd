using System;
using Xunit;

using MarketPlace.Domain.Entities;
using MarketPlace.Domain.ValueObjects;
using MarketPlace.Domain.Exceptions;

namespace MarketPlace.Tests
{
    public class ClassfiedAdTests
    {
        private readonly ClassfiedAd _classfiedAd;
        public ClassfiedAdTests()
        {
            _classfiedAd = new ClassfiedAd(
                new ClassfiedAdId(Guid.NewGuid()),
                new UserId(Guid.NewGuid())
            );
        }
        [Fact]
        public void Can_publish_a_valid_ad()
        {
            _classfiedAd.SetTitle(ClassfiedAdTitle.FromString("Test ad"));
            _classfiedAd.UpdateText(ClassfiedAdText.FromString("Please buy my stuff"));
            _classfiedAd.UpdatePrice(Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));
            _classfiedAd.RequestToPublish();

            Assert.Equal(ClassfiedAd.ClassifiedAdState.PendingReview, _classfiedAd.State);
        }
        [Fact]
        public void Cannot_publish_without_title()
        {
            _classfiedAd.UpdatePrice(Price.FromDecimal(5, "EUR", new FakeCurrencyLookup()));
            _classfiedAd.UpdateText(ClassfiedAdText.FromString("Please by my stuff"));

            Assert.Throws<InvalidEntityStateException>(() => _classfiedAd.RequestToPublish());
        }
        [Fact]
        public void Cannot_publish_without_text()
        {
            _classfiedAd.SetTitle(ClassfiedAdTitle.FromString("Test ad"));
            _classfiedAd.UpdatePrice(Price.FromDecimal(5, "USD", new FakeCurrencyLookup()));

            Assert.Throws<InvalidEntityStateException>(() => _classfiedAd.RequestToPublish());
        }
        [Fact]
        public void Cannot_publish_without_price()
        {
            _classfiedAd.SetTitle(ClassfiedAdTitle.FromString("Test ad"));
            _classfiedAd.UpdateText(ClassfiedAdText.FromString("Please buy my stuff"));

            Assert.Throws<InvalidEntityStateException>(() => _classfiedAd.RequestToPublish());
        }
        [Fact]
        public void Cannot_publish_with_zero_price()
        {
            _classfiedAd.SetTitle(ClassfiedAdTitle.FromString("Test ad"));
            _classfiedAd.UpdatePrice(Price.FromDecimal(0, "USD", new FakeCurrencyLookup()));
            _classfiedAd.UpdateText(ClassfiedAdText.FromString("Please buy my stuff"));

            Assert.Throws<InvalidEntityStateException>(() => _classfiedAd.RequestToPublish());
        }
    }
}
