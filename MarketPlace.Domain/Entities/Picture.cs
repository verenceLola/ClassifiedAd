using System;
using System.Collections.Generic;
using MarketPlace.Framework;
using MarketPlace.Domain.ValueObjects;
using MarketPlace.Domain.Events;

namespace MarketPlace.Domain.Entities
{
    public class Picture : BaseEntity<PictureId>
    {
        internal PictureSize Size { get; set; }
        internal Uri Location { get; set; }
        internal int Order { get; set; }
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.PictureAddedToAClassifiedAd e:
                    Id = new PictureId(e.PictureId);
                    Location = new Uri(e.Url);
                    Size = new PictureSize
                    {
                        Height = e.Height,
                        Width = e.Width
                    };
                    Order = e.Order;
                    break;
                case Events.ClassifiedAdPictureResized e:
                    Size = new PictureSize
                    {
                        Height = e.Height,
                        Width = e.Width,
                    };
                    break;
            }
        }
        public Picture(Action<object> applier) : base(applier) { }
        public void Resize(PictureSize newSize) => Apply(new ClassifiedAdPictureResized
        {
            Height = newSize.Height,
            Width = newSize.Width,
            PictureId = Id.Value
        });
        protected override void EnsureValidState()
        {
            throw new NotImplementedException();
        }
    }

}
