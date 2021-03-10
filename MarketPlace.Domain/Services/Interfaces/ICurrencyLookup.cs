using MarketPlace.Domain.ValueObjects.ClasifiedAd;

namespace MarketPlace.Domain.Services.Interfaces
{
    public interface IcurrencyLookup
    {
        CurrencyDetails FindCurrency(string currencyCode);
    }
}
