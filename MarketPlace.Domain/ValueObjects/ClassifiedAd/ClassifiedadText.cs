using System;
using MarketPlace.Framework;

namespace MarketPlace.Domain.ValueObjects.ClasifiedAd
{
    public class ClassfiedAdText : Value<ClassfiedAdText>
    {
        public static ClassfiedAdText FromString(string text)
        {
            CheckValidity(text);

            return new ClassfiedAdText(text);
        }
        internal ClassfiedAdText(string text) => Value = text;
        public string Value { get; private set; }
        public static implicit operator String(ClassfiedAdText self) => self.Value;
        private static void CheckValidity(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException(nameof(text), "Text must be specified");
            }
        }
        protected ClassfiedAdText() { }
    }
}
