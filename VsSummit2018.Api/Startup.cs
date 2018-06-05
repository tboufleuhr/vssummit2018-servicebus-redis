using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace VsSummit2018.Api
{
    public class Startup
    {
        private IConfiguration Configuration;
        private IHostingEnvironment Environment;
        private IServiceProvider Provider;

        public Startup(IConfiguration configuration, IHostingEnvironment environment, IServiceProvider provider)
        {
            Configuration = configuration;
            Environment = environment;
            Provider = provider;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddOptions();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "VsSummit2018 API",
                    Version = "v1",
                });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.RegisterAppServices(Configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "VsSummit2018 API V1");
            });

            app.UseMvc();

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();
            app.UseDefaultFiles();
        }
    }
}
