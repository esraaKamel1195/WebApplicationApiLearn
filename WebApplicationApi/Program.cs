using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApplicationApi.Models;

namespace WebApplicationApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddDbContext<CompanyContext>(options =>
            options.UseSqlServer("Data Source=DESKTOP-KK73L52;Initial Catalog=testWebApi;Integrated Security=True;"));

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        //swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(swagger =>
        {
            //This is to generate the Default UI of Swagger Documentation    
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ASP.NET 9 Web API",
                Description = " Steps Project"
            });
        });

        /*builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });*/

        /*builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.Equals("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
            });
        });*/

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            //swagger
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowAll");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
