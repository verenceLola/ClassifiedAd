using MarketPlace.Domain.ValueObjects;

namespace MarketPlace.Domain.Services.Interfaces
{
    public interface IcurrencyLookup
    {
        CurrencyDetails FindCurrency(string currencyCode);
    }
}
