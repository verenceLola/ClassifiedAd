using System;
using MarketPlace.Framework;

namespace MarketPlace.Domain.ValueObjects.UserProfile
{
    public class FullName : Value<FullName>
    {
        public string Value { get; }
        internal FullName(string fullName) => Value = fullName;
        public static FullName FromString(string fullName)
        {
            if (fullName.IsEmpty())
            {
                throw new ArgumentNullException(nameof(fullName));
            }

            return new FullName(fullName);
        }
        public static implicit operator string(FullName fullName) =>
            fullName.Value;

        protected FullName() { }
    }
}
