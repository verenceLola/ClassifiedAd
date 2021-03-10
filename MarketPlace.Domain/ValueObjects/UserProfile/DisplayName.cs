using System;
using MarketPlace.Framework;
using MarketPlace.Domain.Interfaces;
using MarketPlace.Domain.Exceptions;

namespace MarketPlace.Domain.ValueObjects.UserProfile
{
    public class DisplayName : Value<DisplayName>
    {
        public string Value { get; }
        internal DisplayName(string displayName) => Value = displayName;
        public static DisplayName FromString(string displayName, CheckTextForProfanity hasProfanity)
        {
            if (displayName.IsEmpty())
            {
                throw new ArgumentNullException(nameof(displayName));
            }
            if (hasProfanity(displayName))
            {
                throw new ProfanityFound(displayName);
            }

            return new DisplayName(displayName);
        }
        public static implicit operator string(DisplayName displayName) =>
            displayName.Value;

        protected DisplayName() { }
    }
}
