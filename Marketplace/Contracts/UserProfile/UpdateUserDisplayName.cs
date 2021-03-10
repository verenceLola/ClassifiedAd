using System;

namespace Marketplace.Contracts.UserProfile
{
    public class UpdateUserDisplayName
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
    }
}
