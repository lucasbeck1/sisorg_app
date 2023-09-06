using Microsoft.EntityFrameworkCore;
using api.Models;
using api.Services;

namespace api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
                
            builder.Services.AddControllers();
            builder.Services.AddScoped<FileService>();
            builder.Services.AddDbContext<SisorgContext>(opt =>
                opt.UseInMemoryDatabase("sisorg_test"));

            // Configure CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:3030")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
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

            app.UseAuthorization();

            // Use CORS policy
            app.UseCors("MyPolicy");

            app.MapControllers();

            app.Run();
        }
    }
}