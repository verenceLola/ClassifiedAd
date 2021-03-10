using System;
using MarketPlace.Framework;

namespace MarketPlace.Domain.ValueObjects
{
    public class UserId : Value<UserId>
    {
        public Guid Value { get; internal set; }
        public UserId(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value), "User id cannot be null");
            }
            Value = value;
        }
        public static implicit operator Guid(UserId self) => self.Value;
        public static implicit operator UserId(string value) => new UserId(Guid.Parse(value));
        protected UserId() { }
    }
}
