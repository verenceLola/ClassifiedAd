using System;
using MarketPlace.Framework;
using MarketPlace.Domain.Exceptions;
using MarketPlace.Domain.Services.Interfaces;

namespace MarketPlace.Domain.ValueObjects.ClasifiedAd
{
    public class Money : Value<Money>
    {
        public decimal Amount { get; private set; }
        public CurrencyDetails Currency { get; private set; }
        protected Money(decimal amount, string currencyCode, IcurrencyLookup currencyLookup)
        {
            if (string.IsNullOrEmpty(currencyCode))
            {
                throw new ArgumentNullException(nameof(currencyCode), "Currency code must be specified");
            }

            CurrencyDetails currency = currencyLookup.FindCurrency(currencyCode);
            if (!currency.InUse)
            {
                throw new ArgumentException($"Currency {currencyCode} is not valid");
            }

            if (decimal.Round(amount, currency.DecimalPlaces) != amount)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), $"Amount in {currencyCode} cannot have more than {currency.DecimalPlaces} decimals");
            }

            Amount = amount;
            Currency = currency;
        }
        public static Money FromDecimal(decimal amount, string currency, IcurrencyLookup currencyLookup) => new Money(amount, currency, currencyLookup);
        public static Money FromString(string amount, string currency, IcurrencyLookup currencyLookup) => new Money(decimal.Parse(amount), currency, currencyLookup);
        protected Money(decimal amount, CurrencyDetails currency)
        {
            Amount = amount;
            Currency = currency;
        }
        protected Money() { }
        public Money Add(Money summand)
        {
            if (summand.Currency != Currency)
            {
                throw new CurrencyMismatchException("Cannot sum amounts with different currencies");
            }

            return new Money(Amount + summand.Amount, Currency);
        }
        public Money Subtract(Money subtrahend)
        {
            if (subtrahend.Currency != Currency)
            {
                throw new CurrencyMismatchException("Cannot subtract amounts with different currencies");
            }

            return new Money(Amount - subtrahend.Amount, Currency);
        }
        public static Money operator +(Money summand1, Money summand2) => summand1.Add(summand2);
        public static Money operator -(Money minuend, Money subtrahend) => minuend.Subtract(subtrahend);
        public override string ToString() => $"{Currency.CurrencyCode} {Amount}";
    }
}
