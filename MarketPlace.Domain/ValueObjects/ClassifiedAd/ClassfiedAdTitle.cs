using System;
using MarketPlace.Framework
;

namespace MarketPlace.Domain.ValueObjects.ClasifiedAd
{
    public class ClassfiedAdTitle : Value<ClassfiedAdTitle>
    {
        public static ClassfiedAdTitle FromString(string title)
        {
            CheckValidity(title);

            return new ClassfiedAdTitle(title);
        }
        public string Value { get; private set; }
        internal ClassfiedAdTitle(string value) => Value = value;
        protected ClassfiedAdTitle() { }
        public static implicit operator String(ClassfiedAdTitle self) => self.Value;
        private static void CheckValidity(string value)
        {
            if (value.Length > 100)
            {
                throw new ArgumentOutOfRangeException("Title cannot be longer than 100 charcaters", nameof(value));
            }
        }
    }
}
