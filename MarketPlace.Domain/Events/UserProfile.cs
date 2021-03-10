using System;

namespace MarketPlace.Domain.Events.UserProfile
{
    public class UserRegistered
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
    }
    public class ProfilePhotoUploaded
    {
        public Guid UserId { get; set; }
        public string PhotoUrl { get; set; }
    }
    public class userFullNameChanged
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
    }
    public class UserDisplayNameUpdated
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
    }
}
