using System;
using MarketPlace.Framework;

namespace MarketPlace.Domain.ValueObjects
{
    public class ClassfiedAdId : Value<ClassfiedAdId>
    {
        public readonly Guid Value;
        public ClassfiedAdId(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value), "Classified Ad id cannot be empty");
            }

            Value = value;
        }
        public static implicit operator Guid(ClassfiedAdId self) => self.Value;
    }
}
