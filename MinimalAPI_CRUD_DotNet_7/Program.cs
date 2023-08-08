using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalAPI_CRUD_DotNet_7.DB;
using MinimalAPI_CRUD_DotNet_7.Minimal_API_Routes;
using MinimalAPI_CRUD_DotNet_7.ViewModal;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using test;
using test.Minimal_API_Routes;
using test.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region ConnectionString
builder.Services.AddDbContext<DbGenricRepositoryPatternContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbCrud"),
    sqlServerOptions => sqlServerOptions.CommandTimeout((int)TimeSpan.FromMinutes(30).TotalSeconds));
});
#endregion

// Register the ApiConfigurationForSpCall class with the service collection
builder.Services.AddScoped<ApiConfigurationForSpCall>();

//Jwt Usefull link
//https://www.infoworld.com/article/3669188/how-to-implement-jwt-authentication-in-aspnet-core-6.html

#region add_authorization_services
builder.Services.AddAuthorization();
#endregion

#region Enabling_authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();
#endregion

//https://code-maze.com/swagger-authorization-aspnet-core/

#region Authorization_with_Swagger_Accepting_Bearer_Token(Jwt_token)
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
#endregion


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
#region  create_JWT_Token_for_authenticated
app.CreateJwtToken();
#endregion


#region MinimalApiEndPoinr
app.Employee1Service();
app.JoinUsingLinqAndLambda();
#endregion

#region CreateCustomClassAndCallUsing_InjectDI
//If you don't want to use static methods, you can achieve API endpoint configuration in a separate
//class without using static methods by using dependency injection.


// Get the instance of ApiConfiguration using dependency injection
//var apiConfiguration = app.Services.GetRequiredService<ApiConfigurationForSpCall>();
//apiConfiguration.ConfigureSpEndpoints(app);
var serviceProvider = app.Services;
var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

using var scope = scopeFactory.CreateScope();
var apiConfiguration = scope.ServiceProvider.GetRequiredService<ApiConfigurationForSpCall>();


//app.UseEndpoints(endpoints =>
//{
//    apiConfiguration.ConfigureSpEndpoints(endpoints);
//});

#endregion

#region authentication_and_authorization_Method
app.UseAuthentication();
app.UseAuthorization();
#endregion

app.UseEndpoints(apiConfiguration.ConfigureSpEndpoints);
app.Run();

#region UpdateCommandForEfCoreModal
//It is mandatory to use --force
//Scaffold - DbContext "Server=DESKTOP-ROD18FU;Database=DB_GenricRepositoryPattern;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer - OutputDir DB - Force
#endregion
