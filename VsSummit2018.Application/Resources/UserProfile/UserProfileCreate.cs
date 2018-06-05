using VsSummit2018.Domain;

namespace VsSummit2018.Application.Resources
{
    public class UserProfileCreate : CommandRequest<UserProfileInfo>
    {
        public string Email { get; set; }
        public string Secret { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}