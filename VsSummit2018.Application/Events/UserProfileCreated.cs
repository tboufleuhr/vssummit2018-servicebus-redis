using VsSummit2018.Domain;

namespace VsSummit2018.Application.Events
{
    public class UserProfileCreated : Event
    {
        public int UserProfileId { get; set; }
        public string FullName { get; set; }

        public override string ToString()
        {
            return $"User '{FullName}' with ID '{UserProfileId}' created";
        }
    }
}
