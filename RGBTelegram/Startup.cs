using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RGBTelegram.Commands;
using RGBTelegram.Services;
using RGBTelegram.vpluse;

namespace RGBTelegram
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
            services.AddControllers();
            services.AddDbContext<DataContext>(opt =>
               opt.UseNpgsql(_configuration.GetConnectionString("Db")), ServiceLifetime.Singleton);
            services.AddSingleton<TelegramBot>();
            services.AddSingleton<AsuBot>();
            services.AddSingleton<PialaBot>();
            services.AddSingleton<PepsiBot>();
            services.AddSingleton<ICommandExecutor, CommandExecutor>();
            services.AddSingleton<IUZCommExecutor, UZCommExecutor>();
            services.AddSingleton<IPepsiCommExecutor, PepsiCommExecutor>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IServiceCall, ServiceCall>();
            services.AddSingleton<IRegService, RegService>();
            services.AddSingleton<ILanguageText, LanguageText>();
            services.AddSingleton<IPepsiText, PepsiText>();
            services.AddSingleton<IRestoreService, RestoreService>();
            services.AddSingleton<BaseCommand, MessageCommands>();
            services.AddSingleton<BaseCommand, CallbackCommands>();
            services.AddSingleton<UZBaseCommand, UZMessageCommands>();
            services.AddSingleton<UZBaseCommand, UZCallbackCommands>();
            services.AddSingleton<PepsiBaseCommand, PepsiMessageCommands>();
            services.AddSingleton<PepsiBaseCommand, PepsiCallbackCommands>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            serviceProvider.GetRequiredService<TelegramBot>().GetBot().Wait();
            serviceProvider.GetRequiredService<AsuBot>().GetASUBot().Wait();
            serviceProvider.GetRequiredService<PialaBot>().GetPialaBot().Wait();
            serviceProvider.GetRequiredService<PepsiBot>().GetPepsiBot().Wait();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
