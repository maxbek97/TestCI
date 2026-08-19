using Microsoft.EntityFrameworkCore;
using TestCI.Aplication.Auth;
using TestCI.Aplication.Auth.Register;
using TestCI.Infrastructure.Authentification;
using TestCI.Infrastructure.Persistence;
using TestCI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DigiRubContext>(options =>
    options.UseNpgsql(
        connectionString,
        o => o.MapEnum<StatusWallet>("status_wallet")
        ));
builder.Services.AddScoped<RegisterHandler>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
