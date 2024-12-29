using BespokeBike.SalesTracker.API.Extensions;
using Serilog;

namespace BespokeBike.SalesTracker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder
                        .WithOrigins("http://127.0.0.1:64153") // Replace with your Angular app's URL
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(Program)); // Add this line

            // Register custom services and repositories
            builder.Services.AddCustomServices(builder.Configuration);

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(); 
            });

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

            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
