using System.Text.Json.Serialization;
using TestCI.Infrastructure;
using TestCI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();
// Авто-миграции
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DigiRubContext>();

    var retries = 10;
    while (retries > 0)
    {
        try
        {
            db.Database.Migrate();
            Console.WriteLine("Migration successful");
            break;
        }
        catch (Exception ex)
        {
            retries--;
            Console.WriteLine("Migration failed: " + ex.Message);
            Thread.Sleep(5000);
        }
    }
}
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
