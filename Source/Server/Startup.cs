using WDS.Authentication;
using WDS.Authorization;
using WDS.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using DataService.Core.Settings;
using DataService.Core.Logging;
using static WDSClient.Models.WDSEnums;

namespace WDS
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment CurrentEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            WDSLogs.AddLog($"Starting up", LogLevels.Information);
            Configuration = configuration;
            CurrentEnvironment = env;
        }
       
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            WDSLogs.AddLog($"Configuring services", LogLevels.Information);
            services.AddSwagger();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = WDSAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = WDSAuthenticationOptions.DefaultScheme;
            }).AddApiKeySupport(options => { });

            services.AddAuthorization(Policies.AddPolicies);
            
            services.AddSingleton<IAuthorizationHandler, DataConsumerAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, DataWriterAuthorizationHandler>();
            services.AddSingleton<IGetApiKeyQuery, InMemoryGetApiKeyQuery>();
            
            services.AddControllers();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            WDSLogs.AddLog($"Loading DataService Settings ({Directory.GetCurrentDirectory()}\\settings)", LogLevels.Information); 
            DataServiceSettings.Load(env, $"{Directory.GetCurrentDirectory()}\\settings");
            
            app.UseSwagger();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //if (!env.IsProduction())
            //{
                app.UseCustomSwagger();
            //}
        }
    }
}
