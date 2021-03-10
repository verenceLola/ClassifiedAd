using System;
using MarketPlace.Framework;

namespace MarketPlace.Domain.ValueObjects
{
    public class UserId : Value<UserId>
    {
        public readonly Guid Value;
        public UserId(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value), "User id cannot be null");
            }
            Value = value;
        }
        public static implicit operator Guid(UserId self) => self.Value;
    }
}
