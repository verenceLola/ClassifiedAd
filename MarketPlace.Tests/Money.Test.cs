using System;
using Xunit;

using MarketPlace.Domain.ValueObjects;
using MarketPlace.Domain.Services.Interfaces;
using MarketPlace.Domain.Exceptions;


namespace MarketPlace.Tests
{
    public class MoneyTest
    {
        private static readonly IcurrencyLookup CurrencyLookup = new FakeCurrencyLookup();
        [Fact]
        public void Two_of_same_amount_should_be_equal()
        {
            Money firstAmount = Money.FromDecimal(5, "EUR", CurrencyLookup);
            Money secondAmount = Money.FromDecimal(5, "EUR", CurrencyLookup);

            Assert.Equal(firstAmount, secondAmount);
        }
        [Fact]
        public void Two_of_same_amount_but_different_currencies_should_not_equal()
        {
            Money firstAmount = Money.FromDecimal(5, "EUR", CurrencyLookup);
            Money secondAmount = Money.FromDecimal(5, "USD", CurrencyLookup);

            Assert.NotEqual(secondAmount, firstAmount);
        }
        [Fact]
        public void FromString_and_FromDecimal_should_be_equal()
        {
            Money firstAmount = Money.FromDecimal(5, "EUR", CurrencyLookup);
            Money secondAmount = Money.FromString("5.00", "EUR", CurrencyLookup);

            Assert.Equal(firstAmount, secondAmount);
        }
        [Fact]
        public void Sum_of_mone_gives_full_amount()
        {
            var coin1 = Money.FromDecimal(1, "EUR", CurrencyLookup);
            var coin2 = Money.FromDecimal(2, "EUR", CurrencyLookup);
            var coin3 = Money.FromDecimal(2, "EUR", CurrencyLookup);
            var bankNote = Money.FromDecimal(5, "EUR", CurrencyLookup);

            Assert.Equal(bankNote, coin1 + coin2 + coin3);
        }
        [Fact]
        public void Unused_currency_should_not_be_allowed()
        {
            Assert.Throws<ArgumentException>(() =>
            Money.FromDecimal(100, "DEM", CurrencyLookup));
        }
        [Fact]
        public void Unknown_currency_should_not_be_allowed()
        {
            Assert.Throws<ArgumentException>(() =>
            Money.FromDecimal(100, "WHAT?", CurrencyLookup));
        }
        [Fact]
        public void Throw_when_too_many_decimal_places()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            Money.FromDecimal(100.123m, "EUR", CurrencyLookup));
        }
        [Fact]
        public void Throws_on_adding_different_currencies()
        {
            var firstAmount = Money.FromDecimal(5, "EUR", CurrencyLookup);
            var secondAmount = Money.FromDecimal(19, "USD", CurrencyLookup);

            Assert.Throws<CurrencyMismatchException>(() => firstAmount.Add(secondAmount));
        }
        [Fact]
        public void Throws_on_subtracting_different_currencies()
        {
            var firstAmount = Money.FromDecimal(15, "USD", CurrencyLookup);
            var secondAmount = Money.FromDecimal(6, "EUR", CurrencyLookup);

            Assert.Throws<CurrencyMismatchException>(() => firstAmount - secondAmount);
        }
    }
}
