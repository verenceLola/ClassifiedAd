using System.Collections.Generic;
using System.Linq;
using MarketPlace.Domain.ValueObjects.ClasifiedAd;
using MarketPlace.Domain.Services.Interfaces;

namespace Marketplace.Infrastructure
{
    public class FixedCurrencyLookup : IcurrencyLookup
    {
        private static readonly IEnumerable<CurrencyDetails> _currencies =
            new[]
            {
                new CurrencyDetails
                {
                    CurrencyCode = "EUR",
                    DecimalPlaces = 2,
                    InUse = true
                },
                new CurrencyDetails
                {
                    CurrencyCode = "USD",
                    DecimalPlaces = 2,
                    InUse = true
                }
            };

        public CurrencyDetails FindCurrency(string currencyCode)
        {
            var currency = _currencies.FirstOrDefault(x => x.CurrencyCode == currencyCode);
            return currency ?? CurrencyDetails.None;
        }
    }
}
