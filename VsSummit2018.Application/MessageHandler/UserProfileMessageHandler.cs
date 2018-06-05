using VsSummit2018.Application.Resources;
using VsSummit2018.Domain;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace VsSummit2018.Application.MessageHandler
{
    public class UserProfileMessageHandler : IMessageHandler<UserProfileCreate>
    {
        private IBus bus;
        private IMessageBroker broker;

        public UserProfileMessageHandler(IBus bus, IMessageBroker broker)
        {
            this.bus = bus;
            this.broker = broker;
        }

        public async Task Handle(UserProfileCreate message)
        {
            var response = await bus.Send(message);
            if (response != null)
            {
                await broker.PublishAsync("UserCreated", response.ToUserProfileCreated());
            }
        }
    }
}

