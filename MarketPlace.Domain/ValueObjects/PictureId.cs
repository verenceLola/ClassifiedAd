using System;
using MarketPlace.Framework;

namespace MarketPlace.Domain.ValueObjects
{
    public class PictureId : Value<PictureId>
    {
        public PictureId(Guid value) => Value = value;
        public Guid Value { get; }
        public PictureId() { }
    }
}
