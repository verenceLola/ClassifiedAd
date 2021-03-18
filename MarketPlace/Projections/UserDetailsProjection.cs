using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Raven.Client.Documents.Session;
using MarketPlace.Domain.Events.UserProfile;

namespace MarketPlace.Projections
{
    public class UserDetailsProjection : RavenDBProjection<ReadModels.UserDetails.UserDetails>
    {
        public UserDetailsProjection(Func<IAsyncDocumentSession> getSession) : base(getSession) { }
        public override Task Project(object @event) =>
            @event switch
            {
                UserRegistered e =>
                    Create(() => Task.FromResult(
                        new ReadModels.UserDetails.UserDetails
                        {
                            UserId = e.UserId,
                            DisplayName = e.DisplayName,
                            Id = e.UserId.ToString()
                        }
                    )),
                UserDisplayNameUpdated e =>
                    UpdateOne(e.UserId, x => x.DisplayName = e.DisplayName),
                ProfilePhotoUploaded e =>
                    UpdateOne(e.UserId, x => x.PhotoUrl = e.PhotoUrl),
                _ => Task.CompletedTask

            };
        }
}
