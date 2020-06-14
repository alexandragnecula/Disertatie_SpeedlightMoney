using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;
using AutoMapper;
using SpeedlightMoney_App.Options;
using System.Reflection;
using DataLayer.SharedInterfaces;
using SpeedlightMoney_App.Services;
using DataLayer;
using SpeedlightMoney_App.Installers;
using BusinessLayer.Services.Users;
using BusinessLayer.Services.Roles;
using BusinessLayer.Services.Currencies;
using BusinessLayer.Common.Mappings;
using BusinessLayer.Services.Debts;
using BusinessLayer.Services.DebtStatuses;
using BusinessLayer.Services.Friends;
using BusinessLayer.Services.Loans;
using BusinessLayer.Services.LoanStatuses;
using BusinessLayer.Services.Terms;
using BusinessLayer.Services.Wallets;
using Quartz.Spi;
using BusinessLayer.Common.SendEmails;
using Quartz;
using Quartz.Impl;

namespace SpeedlightMoney_App
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Cors",
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });

            services.AddInfrastructure(Configuration, Environment);

            services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IDebtService, DebtService>();
            services.AddScoped<IDebtStatusService, DebtStatusService>();
            services.AddScoped<IFriendService, FriendService>();
            services.AddScoped<ILoanService, LoanService>();
            services.AddScoped<ILoanStatusService, LoanStatusService>();
            services.AddScoped<ITermService, TermService>();
            services.AddScoped<IWalletService, WalletService>();

            services.AddHttpContextAccessor();

            services.AddControllers()
                .AddFluentValidation();

          
            services.InstallServicesInAssembly(Configuration);

            // Add Quartz services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // Add our job
            services.AddSingleton<SendEmailJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(SendEmailJob),
                cronExpression: "0 0/1 * 1/1 * ? *")); // (0 0/1 * 1/1 * ? *) runs every 1 minute / (0 0 8 1/1 * ? *) runs every day at 8 am

            services.AddHostedService<QuartzHostedService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("Cors");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}