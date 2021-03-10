using System;


namespace MarketPlace.Domain.Exceptions
{
    public class ProfanityFound : Exception
    {
        public ProfanityFound(string text) : base($"Profanity found in text: {text}") { }
    }
}
