using VsSummit2018.Application.Events;
using VsSummit2018.Domain;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace VsSummit2018.Application.EventHandlers
{
    public class UserProfileEventHandler : IEventHandler<UserProfileCreated>
    {
        public async Task Handle(UserProfileCreated notification, CancellationToken cancellationToken)
        {
            await Task.Run(() => Debug.WriteLine(notification.ToString()));
        }
    }
}
