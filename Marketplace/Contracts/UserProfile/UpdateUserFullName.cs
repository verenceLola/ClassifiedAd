using System;


namespace Marketplace.Contracts.UserProfile
{
    public class UpdateUserFullName
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
    }
}