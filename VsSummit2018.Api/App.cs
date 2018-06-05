using VsSummit2018.Infra;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace VsSummit2018.Api
{
    public class App
    {
        public AppConfiguration Configuration { get; set; }
        public IWebHost WebHost { get; private set; }

        public App(string[] args = null)
        {
            //TODO: Hard coded
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";

            Configuration = new AppConfiguration();

            var hostConfiguration = BuildConfiguration(args, environmentName);
            hostConfiguration.Bind("App", Configuration);
            WebHost = BuildWebHost(hostConfiguration);
        }

        private IConfiguration BuildConfiguration(string[] args, string environmentName) =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName.ToLower()}.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

        private IWebHost BuildWebHost(IConfiguration hostConfiguration) =>
           Microsoft.AspNetCore.WebHost
                .CreateDefaultBuilder()
                .UseConfiguration(hostConfiguration)
                .ConfigureServices(services => services.AddSingleton(this))
                .ConfigureServices(services => services.AddSingleton(Configuration))
                .UseStartup<Startup>()
                .UseUrls(Configuration.Url)
                .ConfigureLogging((hostingContext, builder) =>
                    {
                        builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        builder.AddConsole();
                        builder.AddDebug();
                    })
                .Build();


        public void Run()
        {
            WebHost.Run();
        }

        public void RunAsync()
        {
            WebHost.Start();
        }

        public void Stop()
        {
            WebHost?.StopAsync().Wait();
            WebHost?.Dispose();
        }

        public bool IsDevelopment() =>
            WebHost.Services
                .GetService<IHostingEnvironment>()
                .IsDevelopment();
    }
}
