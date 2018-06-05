using VsSummit2018.Application;
using VsSummit2018.Application.EventHandlers;
using VsSummit2018.Application.Events;
using VsSummit2018.Application.MessageHandler;
using VsSummit2018.Application.Resources;
using VsSummit2018.Data;
using VsSummit2018.Domain;
using VsSummit2018.Domain.Repositories;
using VsSummit2018.Infra;
using VsSummit2018.Infra.MessageBroker;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace VsSummit2018
{
    public static class AppServiceCollectionExtension
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisConfiguration>(configuration.GetSection("App:Redis"));
            services.Configure<MessageBrokerConfiguration>(configuration.GetSection("App:MessageBroker"));
            services.Configure<CachingConfiguration>(configuration.GetSection("App:Caching"));
            services.Configure<DataAccessConfiguration>(configuration.GetSection("App:DataAccess"));

            services.AddScoped<IBus, InProcessBus>();
            services.AddSingleton<ISerializer, MessagePackLessSerializer>();            
            services.AddSingleton<ICache, MemoryCache>();

            services.AddScoped<UserProfileAppService>();

            services.AddScoped<IMessageHandler<UserProfileCreate>, UserProfileMessageHandler>();
            services.AddScoped<IEventHandler<UserProfileCreated>, UserProfileEventHandler>();

            services.AddMemoryCache();

            services.AddMessageBroker();

            services.AddMediatR(
                typeof(IBus).GetTypeInfo().Assembly,
                typeof(UserProfileCreate).GetTypeInfo().Assembly);

            RegisterRepositories(services);

            return services;
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            //services.AddSingleton((s) => {
            //    var configuration = s.GetService<DataAccessConfiguration>();
            //    return new MongoDbContext(configuration.ConnectionString, configuration.Database);
            //});

            services.AddEntityFrameworkNpgsql();

            services.AddDbContext<VsSummit2018Context>((provider, options) => {
                var configuration = provider.GetService<IOptions<DataAccessConfiguration>>().Value;
                options.UseLazyLoadingProxies();
                options.UseNpgsql(configuration.ConnectionString);
                
            });

            services.AddScoped<IUnitOfWork, EntityFrameworkUoW>();
            services.AddScoped(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));
        }
    }
}
