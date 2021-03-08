using System;
using MarketPlace.Framework;

namespace MarketPlace.Domain.ValueObjects
{
    public class UserId : Value<UserId>
    {
        private readonly Guid _value;
        public UserId(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value), "User id cannot be null");
            }
            _value = value;
        }
        public static implicit operator Guid(UserId self) => self._value;
    }
}
