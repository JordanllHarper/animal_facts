using AnimalFactsApi;
using AnimalFactsApi.Model;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AnimalFactsCSVParser;

record DatabaseConfiguration(string DbName, string DbConnectionString, string DbUsername, string DbPassword);

static class Program
{
    private static List<AnimalFact> ParseAnimalFacts(string filepath) =>
        File.ReadAllLines(filepath).Select((line, id) =>
        {
            var contents = line.Split(',');
            Console.WriteLine(contents);
            Console.WriteLine(contents[0]);
            // 5 attributes = 0 - 4
            return new AnimalFact(id, contents[0], contents[1], contents[2], contents[3], contents[4]);
        }).ToList();


    static void AddToDb(IConfiguration configuration, IEnumerable<AnimalFact> facts)
    {
        try
        {
            var animalFactsConfig = configuration.GetSection("AnimalFacts");
            var host = animalFactsConfig.GetValue<string>("DbHost");
            Console.WriteLine($"Using host: {host}");
            var db = animalFactsConfig.GetValue<string>("DbName");
            var username = animalFactsConfig.GetValue<string>("DbUser");
            var password = animalFactsConfig.GetValue<string>("DbPass");
            var connectionString =
                $"Host={host};" +
                $"Database={db};" +
                $"Username={username};" +
                $"Password={password}";

            Console.WriteLine(connectionString);
            var dbContext = new AnimalFactsContext(connectionString);

            dbContext.AddRangeAsync(facts);
            dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    static void Main(string[] args)
    {
        IConfiguration dbConfig = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

        try
        {
            Console.WriteLine($"Parsing: {args.First()}");
            var filename = args.First();
            var facts = ParseAnimalFacts(filename);

            AddToDb(configuration: dbConfig, facts: facts);
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine("No provided file path: please provide the file path you want to parse.");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("No provided file path: please provide the file path you want to parse.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Something went wrong: {e}");
        }
    }
}