using System;
using MarketPlace.Framework;
using MarketPlace.Domain.ValueObjects;
using MarketPlace.Domain.ValueObjects.UserProfile;
using MarketPlace.Domain.Events.UserProfile;

namespace MarketPlace.Domain.Entities
{
    public class UserProfile : AggregateRoot<UserId>
    {
        private string DbId
        {
            get => $"UserProfile{Id.Value}";
            set { }
        }
        public FullName fullName { get; private set; }
        public DisplayName DisplayName { get; private set; }
        public string PhotoUrl { get; private set; }
        public UserProfile(UserId id, FullName fullName, DisplayName displayName) =>
            Apply(new UserRegistered
            {
                UserId = id,
                FullName = fullName,
                DisplayName = displayName
            });
        protected UserProfile() { }

        public void UpdateFullName(FullName fullName) => Apply(new userFullNameChanged
        {
            UserId = Id,
            FullName = fullName
        });
        public void UpdateDisplayName(DisplayName displayName) => Apply(new UserDisplayNameUpdated
        {
            UserId = Id,
            DisplayName = displayName
        });
        public void UpdateProfilePhoto(Uri photoUrl) => Apply(new ProfilePhotoUploaded
        {
            UserId = Id,
            PhotoUrl = photoUrl.ToString()
        });
        protected override void EnsureValidState() { }
        protected override void When(object @event)
        {
            switch (@event)
            {
                case ProfilePhotoUploaded e:
                    PhotoUrl = e.PhotoUrl;
                    break;
                case UserDisplayNameUpdated e:
                    DisplayName = new DisplayName(e.DisplayName);
                    break;
                case UserRegistered e:
                    Id = new UserId(e.UserId);
                    fullName = new FullName(e.FullName);
                    DisplayName = new DisplayName(e.DisplayName);
                    break;
                case userFullNameChanged e:
                    fullName = new FullName(e.FullName);
                    break;
            }
        }
    }
}
