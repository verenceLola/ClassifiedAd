using System;
using System.Threading.Tasks;
using MarketPlace.Domain.Interfaces;
using MarketPlace.Framework;
using MarketPlace.Domain.ValueObjects.UserProfile;
using MarketPlace.Domain.ValueObjects;
using MarketPlace.Domain.ValueObjects.ClasifiedAd;
using MarketPlace.Domain.Entities;


namespace MarketPlace.Services.ApplicationServices
{
    public class UserProfileApplicationService : Interfaces.IApplicationService
    {
        private readonly IAggregateStore _store;

        private readonly CheckTextForProfanity _checkText;
        public UserProfileApplicationService(IAggregateStore store, CheckTextForProfanity checkText)
        {
            _store = store;
            _checkText = checkText;
        }
        private async Task HandleRegister(Contracts.UserProfile.RegisterUser cmd)
        {
            if (await _store.Exists<UserProfile, UserId>(cmd.UserId.ToString()))
            {
                throw new InvalidOperationException($"Entity with id {cmd.UserId} already exists");
            }

            var userProfile = new UserProfile(
                new UserId(cmd.UserId),
                FullName.FromString(cmd.FullName),
                DisplayName.FromString(cmd.DisplayName, _checkText)
            );

            await _store.Save<UserProfile, UserId>(userProfile);
        }
        private async Task HandleUpdate(Guid userId, Action<UserProfile> operation)
            => await this.HandleUpdate(_store, new UserId(userId), operation);
        public Task Handle(object command) =>
            command switch
            {
                Contracts.UserProfile.RegisterUser cmd => HandleRegister(cmd),
                Contracts.UserProfile.UpdateUserDisplayName cmd =>
                    HandleUpdate(cmd.UserId, c => c.UpdateDisplayName(DisplayName.FromString(cmd.DisplayName, _checkText))),
                Contracts.UserProfile.UpdateUserFullName cmd =>
                    HandleUpdate(cmd.UserId, c => c.UpdateFullName(FullName.FromString(cmd.FullName))),
                Contracts.UserProfile.UpdateUserProfilePhoto cmd =>
                    HandleUpdate(cmd.UserId, c => c.UpdateProfilePhoto(new Uri(cmd.PhotUrl))),
                _ => Task.CompletedTask
            };
    }
}
