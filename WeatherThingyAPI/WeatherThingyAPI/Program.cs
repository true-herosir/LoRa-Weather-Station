//using Microsoft.EntityFrameworkCore;
//using WeatherThingyAPI.Models;



//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
////builder.Services.AddDbContext<TodoContext>(options => options.UseInMemoryDatabase("Todolist"));
//// Add services to the container.
//builder.Services.AddDbContext<NodeContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Add controllers and other services.
//builder.Services.AddControllers();

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using WeatherThingyAPI.Models;

namespace WeatherThingyAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline
            Configure(app, app.Environment);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Enable CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Add controllers
            services.AddControllers();

            // Add DbContext with SQL Server
            services.AddDbContext<NodeContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("testing")));

            services.AddDbContext<GatewayContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("testing")));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private static void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            // Swagger and API documentation for development environment
            if (environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // Use CORS
            app.UseCors();

            // Configure HTTPS redirection and authorization
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();
        }
    }
}
