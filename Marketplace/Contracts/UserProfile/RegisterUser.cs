using System;

namespace Marketplace.Contracts.UserProfile
{
    public class RegisterUser
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
    }
}
