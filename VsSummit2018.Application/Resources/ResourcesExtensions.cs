using VsSummit2018.Application.Events;
using VsSummit2018.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace VsSummit2018.Application.Resources
{
    public static class ResourcesExtensions
    {
        public static UserProfileInfo ToUserProfileInfo(this UserProfile userProfile)
        {
            if (userProfile == null)
            {
                return null;
            }

            return new UserProfileInfo()
            {
                UserProfileId = userProfile.Id,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                MiddleName = userProfile.MiddleName,
                FullName = userProfile.FullName,
                Email = userProfile.Email
            };
        }

        public static UserProfileCreated ToUserProfileCreated(this UserProfileInfo userProfileInfo)
        {
            return new UserProfileCreated
            {
                UserProfileId = userProfileInfo.UserProfileId,
                FullName = userProfileInfo.FullName
            };
        }
    }
}
