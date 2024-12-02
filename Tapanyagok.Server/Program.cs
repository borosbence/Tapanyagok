using Microsoft.EntityFrameworkCore;
using Tapanyagok.Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// DbContext hozzáadása
builder.Services.AddDbContext<TapanyagContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("TapanyagDB"), ServerVersion.Parse("10.4.24-mariadb"));
    // SQL paraméterek mutatása a log-ban
    options.EnableSensitiveDataLogging();
});
// CORS engedélyezése
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(
        policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
