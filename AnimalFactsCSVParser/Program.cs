using AnimalFactsApi.Model;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AnimalFactsCSVParser;

record DatabaseConfiguration(string DbName, string DbConnectionString, string DbUsername, string DbPassword);

static class Program
{
    private static List<AnimalFact> ParseAnimalFacts(string filepath) =>
        File.ReadAllLines(filepath).Select(line =>
        {
            var contents = line.Split(',');
            Console.WriteLine(contents);
            Console.WriteLine(contents[0]);
            // 5 attributes = 0 - 4
            return new AnimalFact(contents[0], contents[1], contents[2], contents[3], contents[4]);
        }).ToList();


    static void AddToDb(IConfiguration configuration, IEnumerable<AnimalFact> facts)
    {
        try
        {
            var dbAttr = configuration.GetSection("AnimalFacts");
            var host = dbAttr.GetValue<string>("DbName");
            var username = dbAttr.GetValue<string>("DbUser");
            const string dbName = "postgres";
            const int port = 5432;
            var password = dbAttr.GetValue<string>("DbPassword");


            var connectionString =
                string.Format("Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer", host,
                    username, dbName, port, password);

            using var conn = new NpgsqlConnection(connectionString);
            Console.WriteLine("Opening connection...");
            conn.Open();
            using var createTable = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS facts(" +
                                                      "id serial PRIMARY KEY, " +
                                                      "name VARCHAR(50), " +
                                                      "source VARCHAR(250), " +
                                                      "text VARCHAR(400), " +
                                                      "media VARCHAR(250)," +
                                                      "wiki VARCHAR(250))", conn);
            Console.WriteLine("Connection opened. Creating table...");
            createTable.ExecuteNonQuery();
            Console.WriteLine("Table created, adding data...");
            using var writer = conn.BeginBinaryImport("COPY facts(name, source, text, media, wiki)  FROM STDIN (FORMAT BINARY)");
            foreach (var fact in facts)
            {
                writer.StartRow();
                writer.Write(fact.AnimalName);
                writer.Write(fact.Source);
                writer.Write(fact.Text);
                writer.Write(fact.MediaLink);
                writer.Write(fact.WikiLink);
            }

            writer.Complete();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    static void Main(string[] args)
    {
        IConfiguration dbConfig = new ConfigurationBuilder().AddUserSecrets<DatabaseConfiguration>().Build();

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