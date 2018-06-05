using VsSummit2018.Application.Resources;
using VsSummit2018.Domain;
using VsSummit2018.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VsSummit2018.Application
{
    public class UserProfileAppService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMessageBroker broker;
        private readonly IRepository<UserProfile> userProfileRepository;

        public UserProfileAppService(
            IUnitOfWork unitOfWork,
            IMessageBroker broker,
            IRepository<UserProfile> userProfileRepository)
        {
            this.unitOfWork = unitOfWork;
            this.broker = broker;
            this.userProfileRepository = userProfileRepository;
        }

        private async Task<UserProfile> EnsureGetUserProfile(int id)
        {
            var userProfile = await userProfileRepository.GetByIdAsync(id);
            if (userProfile == null)
            {
                throw new InvalidOperationException($"UserProfile with ID={id} not found.");
            }

            return userProfile;
        }

        public async Task<UserProfileInfo> CreateAsync(UserProfileCreate createCommand)
        {
            var userProfile = new UserProfile()
            {
                Email = createCommand.Email,
                FirstName = createCommand.FirstName,
                MiddleName = createCommand.MiddleName,
                LastName = createCommand.LastName,
            };

            await userProfileRepository.AddAsync(userProfile);

            await unitOfWork.CompleteAsync();

            return userProfile.ToUserProfileInfo();
        }

        public async Task<UserProfileInfo> GetAsync(int id)
        {
            var userProfile = await userProfileRepository.GetByIdAsync(id);
            if (userProfile == null)
            {
                return null;
            }

            return userProfile.ToUserProfileInfo();
        }

        public async Task UpdateAsync(UserProfileUpdateInfo updateCommand)
        {
            var userProfile = await EnsureGetUserProfile(updateCommand.UserProfileId);

            userProfile.FirstName = updateCommand.FirstName;
            userProfile.MiddleName = updateCommand.MiddleName;
            userProfile.LastName = updateCommand.LastName;

            await userProfileRepository.UpdateAsync(userProfile);
            await unitOfWork.CompleteAsync();
        }

        public bool Exists(int id)
        {
            return GetAsync(id).Result != null;
        }
    }
}
