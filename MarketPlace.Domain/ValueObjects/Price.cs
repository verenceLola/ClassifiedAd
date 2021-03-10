using System;
using MarketPlace.Domain.Services.Interfaces;


namespace MarketPlace.Domain.ValueObjects
{
    public class Price : Money
    {
        private Price(decimal amount, string currencyCode, IcurrencyLookup currencyLookup) : base(amount, currencyCode, currencyLookup)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Price cannot be negative", nameof(amount));
            }
        }
        internal Price(decimal amount, string currencyCode) : base(amount, new CurrencyDetails { CurrencyCode = currencyCode }) { }
        public new static Price FromDecimal(decimal amount, string currency, IcurrencyLookup currencyLookup) => new Price(amount, currency, currencyLookup);
        protected Price() { }
    }
}
