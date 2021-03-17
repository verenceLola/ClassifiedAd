using System;

namespace MarketPlace.ReadModels.UserDetails
{
    public class UserDetails
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string PhotoUrl { get; set; }
    }
}
