using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserApi.Api.Middlewares;
using UserApi.Application.Interfaces;
using UserApi.Application.Services;
using UserApi.DataAccess;
using UserApi.DataAccess.Interfaces;
using UserApi.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();
ConfigureServices(builder.Services);
builder.Services.AddTransient<CustomExceptionHandlingMiddleware>();




var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<CustomExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


void ConfigureServices(IServiceCollection services)
{
    services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = true;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("placeholder-key-that-is-long-enough-for-sha256")),
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true
            };
        });

    services.AddScoped<ITokenBuilder, TokenBuilder>();
}