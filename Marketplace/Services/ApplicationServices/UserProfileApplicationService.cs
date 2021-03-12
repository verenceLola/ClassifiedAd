using System;
using System.Threading.Tasks;
using MarketPlace.Domain.Interfaces;
using MarketPlace.Framework;
using MarketPlace.Domain.ValueObjects.UserProfile;
using MarketPlace.Domain.ValueObjects;
using MarketPlace.Domain.Entities;


namespace Marketplace.Api.ApplicationServices
{
    public class UserProfileApplicationService : Interfaces.IApplicationService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CheckTextForProfanity _checkText;
        public UserProfileApplicationService(IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork, CheckTextForProfanity checkText)
        {
            _unitOfWork = unitOfWork;
            _userProfileRepository = userProfileRepository;
            _checkText = checkText;
        }
        private async Task HandleRegister(Contracts.UserProfile.RegisterUser cmd)
        {
            if (await _userProfileRepository.Exists(cmd.UserId.ToString()))
            {
                throw new InvalidOperationException($"Entity with id {cmd.UserId} already exists");
            }

            var userProfile = new UserProfile(
                new UserId(cmd.UserId),
                FullName.FromString(cmd.FullName),
                DisplayName.FromString(cmd.DisplayName, _checkText)
            );

            await _userProfileRepository.Add(userProfile);
            await _unitOfWork.Commit();
        }
        private async Task HandleUpdate(Guid userId, Action<UserProfile> operation)
        {
            var userProfile = await _userProfileRepository.Load(userId.ToString());
            if (userProfile == null)
            {
                throw new InvalidOperationException($"Entity with id {userId} cannot be found");
            }

            operation(userProfile);

            await _unitOfWork.Commit();
        }
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
