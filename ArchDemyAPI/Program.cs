using API.DI;
using Application.Helpers;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using API.Middlewares;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Application.DTOs;
using Stripe;
using Domain.Entities.Token;
using Domain.Entities.Stripe;
using Domain.Entities.PayStack;
using System.Threading.RateLimiting;
using Identity.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtParameters>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<EmailSender>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<EmailVerificationUrls>(builder.Configuration.GetSection("EmailVerificationUrls"));
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));
builder.Services.Configure<PayStackSettings>(builder.Configuration.GetSection("PayStackSettings"));
builder.Services.Configure<GoogleTwoFactorAuthSettings>(builder.Configuration.GetSection("GoogleTwoFactorAuthSettings"));
builder.Services.Configure<TwilioFnParameters>(builder.Configuration.GetSection("TwilioFnParameters"));

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacContainerModule()));

builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureSqlContext(builder.Configuration);//add database
builder.Services.ConfigureSwagger();
//builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();


//builder.Services.AddScoped<HttpContextAccessor>().AddScoped<ITwoFactorAuthService, TwoFactorAuthService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.ConfigureApiVersioning(builder.Configuration);
builder.Services.AddStripeInfrastructure(builder.Configuration);
builder.Services.ConfigureMvc();//register automapper
builder.Services.ConfigureRateLimiting();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();    
    app.UseSwaggerUI(c =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach(var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                       description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseErrorHandler();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseRateLimiter();

app.Run();
