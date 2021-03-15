using System;
using System.Linq;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;
using System.Collections.Generic;
using MarketPlace.Framework;
using MarketPlace.Domain.Events.ClassifiedAdEvents;
using MarketPlace.Domain.Events.UserProfile;

namespace MarketPlace.Projections
{
    public class ClassifiedAdDetailProjection : IProjection
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ClassifiedAdDetailProjection>();
        private readonly IList<ReadModels.ClassifiedAds.ClassfiedAdDetails> _items;
        public ClassifiedAdDetailProjection(IList<ReadModels.ClassifiedAds.ClassfiedAdDetails> items)
        {
            _items = items;
        }

        public Task Project(object @event)
        {
            switch (@event)
            {
                case ClassifiedAdCreated e:
                    _items.Add(new ReadModels.ClassifiedAds.ClassfiedAdDetails
                    {
                        ClassifiedAdId = e.Id,
                        SellerId = e.OwnerId,
                    });
                    break;
                case ClassifiedAdTitleChanged e:
                    UpdateItem(e.Id, ad => ad.Title = e.Title);
                    break;
                case ClassfiedAdTextUpdated e:
                    UpdateItem(e.Id, ad => ad.Description = e.AdText);
                    break;
                case ClassfiedAdPriceUpdated e:
                    UpdateItem(e.Id, ad =>
                    {
                        ad.Price = e.Price;
                        ad.CurrencyCode = e.CurrencyCode;
                    });
                    break;
                case UserDisplayNameUpdated e:
                    UpdateMultipleItems(x => x.SellerId == e.UserId, x => x.SellerDisplayName = e.DisplayName);
                    break;
            }

            return Task.CompletedTask;
        }
        private void UpdateItem(Guid id, Action<ReadModels.ClassifiedAds.ClassfiedAdDetails> update)
        {
            var item = _items.FirstOrDefault(x => x.ClassifiedAdId == id);
            if (item == null) return;

            update(item);
        }
        private void UpdateMultipleItems(Func<ReadModels.ClassifiedAds.ClassfiedAdDetails, bool> query, Action<ReadModels.ClassifiedAds.ClassfiedAdDetails> update)
        {
            foreach (var item in _items.Where(query))
            {
                update(item);
            }
        }
    }
}
