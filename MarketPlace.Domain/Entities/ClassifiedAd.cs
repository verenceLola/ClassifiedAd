using MarketPlace.Domain.ValueObjects;
using MarketPlace.Domain.Exceptions;

namespace MarketPlace.Domain.Entities
{
    public class ClassfiedAd : BaseEntity
    {
        public ClassfiedAdId Id { get; private set; }
        public UserId OwnerId { get; private set; }
        public UserId ApprovedBy { get; private set; }
        public ClassfiedAdTitle Title { get; private set; }
        public ClassfiedAdText Text { get; private set; }
        public Price Price { get; private set; }
        public ClassifiedAdState State { get; private set; }
        public ClassfiedAd(ClassfiedAdId id, UserId ownerId) => Apply(new Events.ClassifiedAdCreated
        {
            Id = id,
            OwnerId = ownerId
        });
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
                ClassifiedAdState.PendingReview => Title != null && Text != null && Price?.Amount > 0,
                ClassifiedAdState.Active => Title != null && Text != null && Price?.Amount > 0 && ApprovedBy != null,
                _ => true
            });

            if (!valid)
            {
                throw new InvalidEntityStateException(this, $"Post-checks failed in state {State}");
            }
        }
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
            }
        }
    }
}
