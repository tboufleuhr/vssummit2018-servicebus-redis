using BlueFix.Application.Events;
using BlueFix.Application.Resources;
using BlueFix.Domain;
using BlueFix.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace BlueFix.BackendServices
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando serviços de backend...");

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";

            var appConfiguration = new AppConfiguration();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName.ToLower()}.json", optional: true)
                .Build();

            configuration.Bind("App", appConfiguration);

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .RegisterAppServices(configuration)
                .BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var broker = scope.ServiceProvider.GetService<IMessageBroker>();
                broker.ReceiveAsync<UserProfileCreate>();
                broker.SubscribeAsync<UserProfileCreated>("UserCreated");

                Console.ReadKey();
            }
        }
    }
}
