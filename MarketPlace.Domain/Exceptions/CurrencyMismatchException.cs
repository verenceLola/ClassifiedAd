using System;

namespace MarketPlace.Domain.Exceptions
{
    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException(string message) : base(message) { }
    }
}
