using System;
using MarketPlace.Framework;

namespace MarketPlace.Domain.ValueObjects.ClasifiedAd
{
    public class ClassfiedAdId : Value<ClassfiedAdId>
    {
        public Guid Value { get; internal set; }
        public ClassfiedAdId(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value), "Classified Ad id cannot be empty");
            }

            Value = value;
        }
        public static implicit operator Guid(ClassfiedAdId self) => self.Value;
        public static implicit operator ClassfiedAdId(string value) =>
            new ClassfiedAdId(Guid.Parse(value));
        public ClassfiedAdId() { }
    }
}
