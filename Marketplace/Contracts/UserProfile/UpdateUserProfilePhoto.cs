using System;

namespace Marketplace.Contracts.UserProfile
{
    public class UpdateUserProfilePhoto
    {
        public Guid UserId { get; set; }
        public string PhotUrl { get; set; }
    }
}
