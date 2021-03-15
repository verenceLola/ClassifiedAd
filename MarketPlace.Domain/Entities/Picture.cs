using System;
using MarketPlace.Domain.ValueObjects.ClasifiedAd;
using MarketPlace.Domain.Events.ClassifiedAdEvents;

namespace MarketPlace.Domain.Entities
{
    public class Picture : BaseEntity<PictureId>
    {
        public PictureSize Size { get; private set; }
        internal Uri Location { get; set; }
        internal int Order { get; set; }
        public Guid PictureId { get => Id.Value; set { } }
        public ClassfiedAdId ParentId { get; private set; }
        protected override void When(object @event)
        {
            switch (@event)
            {
                case PictureAddedToAClassifiedAd e:
                    Id = new PictureId(e.PictureId);
                    Location = new Uri(e.Url);
                    Size = new PictureSize
                    {
                        Height = e.Height,
                        Width = e.Width
                    };
                    Order = e.Order;
                    break;
                case ClassifiedAdPictureResized e:
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
        protected override void EnsureValidState() { }
        public Picture() { }
    }

}
