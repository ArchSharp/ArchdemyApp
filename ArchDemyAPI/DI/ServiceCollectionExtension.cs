using API.Middlewares;
using Application.Mapper;
using Application.Services.Implementations;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShareLoanApp.Application.Services.Interfaces;

namespace API.DI
{
    public static class ServiceCollectionExtension
    {
        private static readonly ILoggerFactory ContextLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        [Obsolete]
        public static void ConfigureMvc(this IServiceCollection services)
        {
            /*services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Latest)
                .ConfigureApiBehaviorOptions(o =>
                {
                    o.InvalidModelStateResponseFactory = context => new ValidationFailedResult(context.ModelState);
                }).AddFluentValidation(x =>
                    x.RegisterValidatorsFromAssemblyContaining<UserValidator>());*/
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(UserMapper));
        }

        //add database
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            var constring = configuration.GetConnectionString("DefaultSQLConnection");
            services.AddDbContext<AppDbContext>(opts =>
                opts
                    .UseLoggerFactory(ContextLoggerFactory)
                    // .UseSqlServer(constring));
                    .UseNpgsql(constring));
                    
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddScoped<ILoggerManager, LoggerManager>();
        }
    }
}
