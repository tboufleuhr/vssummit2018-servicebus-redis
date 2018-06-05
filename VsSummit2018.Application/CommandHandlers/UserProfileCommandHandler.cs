using VsSummit2018.Application.Resources;
using VsSummit2018.Domain;
using VsSummit2018.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace VsSummit2018.Application.CommandHandlers
{
    public class UserProfileCommandHandler : ICommandRequestHandler<UserProfileCreate, UserProfileInfo>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<UserProfile> userProfileRepository;
     
        public UserProfileCommandHandler(
            IUnitOfWork unitOfWork,
            IRepository<UserProfile> userProfileRepository)
        {
            this.unitOfWork = unitOfWork;
            this.userProfileRepository = userProfileRepository;
        }

        public async Task<UserProfileInfo> Handle(UserProfileCreate request, CancellationToken cancellationToken)
        {
            var userProfile = new UserProfile()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
            };

            await userProfileRepository.AddAsync(userProfile);

            await unitOfWork.CompleteAsync();

            return userProfile.ToUserProfileInfo();
        }
    }
}
