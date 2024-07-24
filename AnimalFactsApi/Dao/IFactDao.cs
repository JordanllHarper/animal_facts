using AnimalFactsApi.Model;
using FluentResults;
using Npgsql;

namespace AnimalFactsApi.Dao;

public interface IFactDao
{
    Task<Result<AnimalFact>> getFact();
    Task<Result<AnimalFact>> getFactById(string id);
}

public class FactDao(string connectionString) : IFactDao
{
    public async Task<Result<AnimalFact>> getFact()
    {
        await using var conn = new NpgsqlConnection(connectionString: connectionString);
        Console.WriteLine("Opening connection for getting facts");

        await conn.OpenAsync();
        await using var command = new NpgsqlCommand("SELECT * FROM facts ORDER BY random() LIMIT 1", conn);
        var reader = command.ExecuteReader();
        await reader.ReadAsync();
        var id = reader.GetInt32(0);
        var name = reader.GetString(1);
        var source = reader.GetString(2);
        var text = reader.GetString(3);
        var media = reader.GetString(4);
        var wiki = reader.GetString(5);

        return new AnimalFact(id, name, source, text, media, wiki);
    }


    public async Task<Result<AnimalFact>> getFactById(string id)
    {
        try
        {
            var parsedId = int.Parse(id);
            await using var conn = new NpgsqlConnection(connectionString: connectionString);
            Console.WriteLine("Opening connection for getting facts");

            await conn.OpenAsync();
            await using var command = new NpgsqlCommand($"SELECT * FROM facts WHERE id = {parsedId}", conn);
            var reader = command.ExecuteReader();
            await reader.ReadAsync();
            var factId = reader.GetInt32(0);
            var name = reader.GetString(1);
            var source = reader.GetString(2);
            var text = reader.GetString(3);
            var media = reader.GetString(4);
            var wiki = reader.GetString(5);

            return new AnimalFact(factId, name, source, text, media, wiki);
        }
        catch (FormatException e)
        {
            Console.WriteLine(e);
            return Result.Fail("Id wasn't in correct format");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e);
            return Result.Fail(
                "An item with that id was not found. " +
                "Please try using a valid id or omit the id for a random fact!"
            );
        }
    }
}