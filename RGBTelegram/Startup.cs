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
            services.AddControllers();
            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(_configuration.GetConnectionString("Db")), ServiceLifetime.Singleton);
            services.AddSingleton<TelegramBot>();
            services.AddSingleton<ICommandExecutor, CommandExecutor>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<BaseCommand, MessageCommands>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}