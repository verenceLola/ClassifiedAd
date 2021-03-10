using System.Linq;
using System.Collections.Generic;
using MarketPlace.Domain.Services.Interfaces;
using MarketPlace.Domain.ValueObjects.ClasifiedAd;

namespace MarketPlace.Tests
{
    public class FakeCurrencyLookup : IcurrencyLookup
    {
        private static readonly IEnumerable<CurrencyDetails> _currencies = new[]{
            new CurrencyDetails{
                CurrencyCode = "EUR",
                DecimalPlaces = 2,
                InUse = true
            },
            new CurrencyDetails {
                CurrencyCode = "USD",
                DecimalPlaces = 2,
                InUse = true
            },
            new CurrencyDetails{
                CurrencyCode = "JPY",
                DecimalPlaces = 0,
                InUse = true
            },
            new CurrencyDetails{
                CurrencyCode = "DEM",
                DecimalPlaces = 2,
                InUse = false
            }
        };
        public CurrencyDetails FindCurrency(string currencyCode)
        {
            CurrencyDetails currency = _currencies.FirstOrDefault(
                x => x.CurrencyCode == currencyCode
            );

            return currency ?? CurrencyDetails.None;
        }
    }
}
