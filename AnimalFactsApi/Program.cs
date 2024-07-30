using System.Configuration;
using AnimalFactsApi.Dao;
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
        var connectionString = builder.Configuration.GetConnectionString("AnimalFactsDb");
        builder.Services.AddSingleton<IFactDao, FactDao>(_ =>
        {
            if (connectionString == null)
            {
                throw new ConfigurationErrorsException("Please configure your connection string for postgresql");
            }
            return new FactDao(connectionString);
        });
        builder.Services.AddSingleton<IFactRepository, FactRepository>();
        var animalFactsConfig = builder.Configuration.GetSection("AnimalFacts");
        var connectionString =
            $"Host={animalFactsConfig.GetValue<string>("DbHost")};" +
            $"Database={animalFactsConfig.GetValue<string>("DbName")};" +
            $"Username={animalFactsConfig.GetValue<string>("DbUser")};" +
            $"Password={animalFactsConfig.GetValue<string>("DbPassword")}";
        builder.Services.AddDbContext<AnimalFactsContext>(options =>
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention()
        );
        builder.Configuration.AddUserSecrets<DatabaseConfiguration>();

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
        app.Run();
    }
}