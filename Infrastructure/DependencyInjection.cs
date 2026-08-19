using Microsoft.EntityFrameworkCore;
using TestCI.Aplication.Auth;
using TestCI.Aplication.Auth.Login;
using TestCI.Aplication.Auth.Refresh;
using TestCI.Aplication.Auth.Register;
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

            services.AddDbContext<DigiRubContext>(options =>
    options.UseNpgsql(
        connectionString,
        o => o.MapEnum<StatusWallet>("status_wallet")
        ));


            services.Configure<AuthSettings>(options =>
            {
                // Затягиваем секцию из appsettings.json
                configuration.GetSection("AuthSettings").Bind(options);

                // Подменяем/дописываем SecretKey из переменных окружения, если он там есть
                options.SecretKey = Environment.GetEnvironmentVariable("SECRETKEY")
                                    ?? options.SecretKey;
            });

            services.AddScoped<RegisterHandler>();
            services.AddScoped<RefreshHandler>();
            services.AddScoped<LoginHandler>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher, PasswordService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IJwtService, JwtService>();


            return services;
        }
    }
}
