using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TestCI.Aplication.Auth;
using TestCI.Aplication.Auth.Login;
using TestCI.Aplication.Auth.Refresh;
using TestCI.Aplication.Auth.Register;
using TestCI.Aplication.Clients;
using TestCI.Application.Clients.GetClients;
using TestCI.Domain.DrWallets;
using TestCI.Infrastructure.Authentification;
using TestCI.Infrastructure.Persistence;
using TestCI.Models;

namespace TestCI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            // Если не локально
            var dockerConnectionString = Environment.GetEnvironmentVariable("POSTGRES_HOST") != null
                ? $"Host={Environment.GetEnvironmentVariable("POSTGRES_HOST")};" +
                  $"Port=5432;" +
                  $"Database={Environment.GetEnvironmentVariable("POSTGRES_DB")};" +
                  $"Username={Environment.GetEnvironmentVariable("POSTGRES_USER")};" +
                  $"Password={Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")};"
                : null;

            // 2) Если локально
            var connectionString = dockerConnectionString
                                   ?? configuration.GetConnectionString("DefaultConnection");

            var secretKey = Environment.GetEnvironmentVariable("SECRETKEY")
                    ?? configuration["AuthSettings:SecretKey"]
                    ?? throw new InvalidOperationException("JWT SecretKey is missing!");

            services.AddDbContext<DigiRubContext>(options =>
                options.UseNpgsql(
                    connectionString,
                    o => o.MapEnum<StatusWallet>("status_wallet")
                ));

            services.Configure<AuthSettings>(options =>
            {
                // Затягиваем секцию из appsettings.json
                configuration.GetSection("AuthSettings").Bind(options);
                options.SecretKey = secretKey;
            });

            services.AddScoped<RegisterHandler>();
            services.AddScoped<RefreshHandler>();
            services.AddScoped<LoginHandler>();
            services.AddScoped<GetClientsHandler>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher, PasswordService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey!))
    };
});

            return services;
        }
    }
}
