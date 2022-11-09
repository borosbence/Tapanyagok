using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tapanyagok.API.Models;

namespace Tapanyagok.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // DbContext
            builder.Services.AddDbContext<TapanyagContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("TapanyagDB"), ServerVersion.Parse("10.4.24-mariadb")));

            // CORS
            builder.Services.AddCors(options => 
                options.AddDefaultPolicy(
                    policy => policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod()));

            var app = builder.Build();

            app.UseCors();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}