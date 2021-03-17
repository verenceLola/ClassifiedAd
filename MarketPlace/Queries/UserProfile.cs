using System;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;


namespace MarketPlace
{
    public static class UserProfileQueries
    {
        public static Task<ReadModels.UserDetails.UserDetails> GetUserDetails(
            this Func<IAsyncDocumentSession> getSession,
            Guid id
        )
        {
            using var sesion = getSession();

            return sesion.LoadAsync<ReadModels.UserDetails.UserDetails>(id.ToString());
        }
    }
}
