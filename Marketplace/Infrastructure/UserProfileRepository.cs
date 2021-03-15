using MarketPlace.Framework;
using MarketPlace.Domain.Entities;
using MarketPlace.Domain.ValueObjects;
using MarketPlace.Domain.Interfaces;

using Raven.Client.Documents.Session;


namespace MarketPlace.Infrastructure
{
    public class UserProfileRepository : RavenDBRepository<UserProfile, UserId>, IUserProfileRepository
    {
        public UserProfileRepository(IAsyncDocumentSession session) : base(session, id => $"UserProfile/{id.Value.ToString()}") { }
    }
}
