using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using MarketPlace.Framework;
using MarketPlace.Domain.Events.UserProfile;

namespace MarketPlace.Projections
{
    public class UserDetailsProjection : IProjection
    {
        List<ReadModels.UserDetails.UserDetails> _items;
        public UserDetailsProjection(List<ReadModels.UserDetails.UserDetails> items)
        {
            _items = items;
        }
        public Task Project(object @event)
        {
            switch (@event)
            {
                case UserRegistered e:
                    _items.Add(new ReadModels.UserDetails.UserDetails
                    {
                        UserId = e.UserId,
                        DisplayName = e.DisplayName
                    });
                    break;
                case UserDisplayNameUpdated e:
                    UpdateItem(e.UserId, x => x.DisplayName = e.DisplayName);
                    break;
            }

            return Task.CompletedTask;
        }
        private void UpdateItem(Guid id, Action<ReadModels.UserDetails.UserDetails> update)
        {
            var item = _items.FirstOrDefault(x => x.UserId == id);
            if (item == null) return;

            update(item);
        }
    }
}
