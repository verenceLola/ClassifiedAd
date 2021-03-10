using System;
using MarketPlace.Domain.ValueObjects;
using MarketPlace.Domain.Exceptions;
using MarketPlace.Framework;
using MarketPlace.Domain.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Domain.Entities
{
    public class ClassfiedAd : AggregateRoot<ClassfiedAdId>
    {
        public UserId OwnerId { get; private set; }
        public Guid ClassifiedAdId { get; private set; }
        public UserId ApprovedBy { get; private set; }
        public ClassfiedAdTitle Title { get; private set; }
        public ClassfiedAdText Text { get; private set; }
        public Price Price { get; private set; }
        public ClassifiedAdState State { get; private set; }
        public List<Picture> Pictures { get; private set; }
        private string DbId
        {
            get => $"ClassifiedAd/{Id.Value}";
            set { }
        }
        public ClassfiedAd(ClassfiedAdId id, UserId ownerId)
        {
            Pictures = new List<Picture>();
            Apply(new Events.ClassifiedAdCreated
            {
                Id = id,
                OwnerId = ownerId
            });
        }
        protected ClassfiedAd() { }
        public void SetTitle(ClassfiedAdTitle title) => Apply(new Events.ClassifiedAdTitleChanged
        {
            Id = Id,
            Title = title
        });
        public void UpdateText(ClassfiedAdText text) => Apply(new Events.ClassfiedAdTextUpdated
        {
            Id = Id,
            AdText = text
        });
        public void UpdatePrice(Price price) => Apply(new Events.ClassfiedAdPriceUpdated
        {
            Price = price.Amount,
            CurrencyCode = price.Currency.CurrencyCode,
            Id = Id
        });
        public enum ClassifiedAdState
        {
            PendingReview, Active, Inactive, MarkedAsSold
        };
        public void RequestToPublish() => Apply(new Events.ClassifiedAdSentForReview
        {
            Id = Id
        });

        protected override void EnsureValidState()
        {
            bool valid = Id != null && OwnerId != null && (State switch
            {
                ClassifiedAdState.PendingReview => Title != null && Text != null && Price?.Amount > 0 && FirstPicture.HasCorrectSize(),
                ClassifiedAdState.Active => Title != null && Text != null && Price?.Amount > 0 && FirstPicture.HasCorrectSize() && ApprovedBy != null,
                _ => true
            });

            if (!valid)
            {
                throw new InvalidEntityStateException(this, $"Post-checks failed in state {State}");
            }
        }
        public void AddPicture(Uri pictureUri, PictureSize size) => Apply(new Events.PictureAddedToAClassifiedAd
        {
            PictureId = new Guid(),
            ClassifiedAdId = Id,
            Url = pictureUri.ToString(),
            Height = size.Height,
            Width = size.Width,
            Order = Pictures.Max(x => x.Order)
        });
        public void ResizePicture(PictureId pictureId, PictureSize newSize)
        {
            var picture = FindPicture(pictureId);

            if (picture == null)
            {
                throw new InvalidOperationException("Cannot resize a picture that I don't have");
            }

            picture.Resize(newSize);
        }
        private Picture FindPicture(PictureId id) => Pictures.FirstOrDefault(x => x.Id == id);
        private Picture FirstPicture => Pictures.OrderBy(x => x.Order).FirstOrDefault();
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.ClassifiedAdCreated e:
                    Id = new ClassfiedAdId(e.Id);
                    OwnerId = new UserId(e.OwnerId);
                    State = ClassifiedAdState.Inactive;
                    break;
                case Events.ClassfiedAdPriceUpdated e:
                    Id = new ClassfiedAdId(e.Id);
                    Price = new Price(e.Price, e.CurrencyCode);
                    break;
                case Events.ClassfiedAdTextUpdated e:
                    Id = new ClassfiedAdId(e.Id);
                    Text = new ClassfiedAdText(e.AdText);
                    break;
                case Events.ClassifiedAdTitleChanged e:
                    Id = new ClassfiedAdId(e.Id);
                    Title = new ClassfiedAdTitle(e.Title);
                    break;
                case Events.ClassifiedAdSentForReview e:
                    State = ClassifiedAdState.PendingReview;
                    break;
                case Events.PictureAddedToAClassifiedAd e:
                    var newPicture = new Picture(Apply);
                    Pictures.Add(newPicture);
                    break;
            }
        }
    }
}
