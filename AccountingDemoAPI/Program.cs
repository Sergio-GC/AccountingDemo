
using System.Security.Cryptography;
using System.Text;
using BLLAccountingDemo;
using BLLAccountingDemo.Mapping;
using EFAccounting;
using Microsoft.EntityFrameworkCore;

namespace AccountingDemoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<Context>(options =>
                options.UseLazyLoadingProxies()
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
            );


            builder.Services.AddScoped<KidManager>();
            builder.Services.AddScoped<WDayManager>();
            builder.Services.AddScoped<PriceManager>();

            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            // Use the api key from the configuration file to check the authenticity of the requests
            app.Use(async (context, next) =>
            {
                string expectedKey = app.Configuration["Api:Key"] ?? string.Empty;
                if (!context.Request.Headers.TryGetValue("X-API-Key", out var key) ||
                    !CryptographicOperations.FixedTimeEquals(
                        Encoding.UTF8.GetBytes(key.ToString()),
                        Encoding.UTF8.GetBytes(expectedKey)
                    ))
                {
                    context.Response.StatusCode = 401;
                    return;
                }

                await next(); 
            });

            app.Run();
        }
    }
}
