using System.Configuration;
using AnimalFactsApi.Dao;
using AnimalFactsApi.Repo;
using Microsoft.EntityFrameworkCore;

namespace AnimalFactsApi;

record DatabaseConfiguration(string DbName, string DbConnectionString, string DbUsername, string DbPassword);

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        var animalFactsConfig = builder.Configuration.GetSection("AnimalFacts");
        var connectionString =
            $"Host={animalFactsConfig.GetValue<string>("DbHost")};" +
            $"Database={animalFactsConfig.GetValue<string>("DbName")};" +
            $"Username={animalFactsConfig.GetValue<string>("DbUser")};" +
            $"Password={animalFactsConfig.GetValue<string>("DbPassword")}";
        builder.Services.AddSingleton<IFactDao, FactDao>();
        builder.Services.AddSingleton<IFactRepository, FactRepository>();
        builder.Services.AddDbContext<AnimalFactsContext>(options =>
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention()
        );

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            builder.Configuration.AddJsonFile(path: "appsettings.Development.json");
        }
        else
        {
            builder.Configuration.AddUserSecrets<DatabaseConfiguration>();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}