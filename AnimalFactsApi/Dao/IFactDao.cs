using AnimalFactsApi.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AnimalFactsApi.Dao;

public interface IFactDao
{
    Task<Result<AnimalFact>> getFact();
    Task<Result<AnimalFact>> getFactById(string id);
}

public class FactDao(AnimalFactsContext context) : IFactDao
{
    private AnimalFactsContext _context = context;

    public Task<Result<AnimalFact>> getFact()
    {
        
        return context.Facts.FromSqlInterpolated($"SELECT * FROM animal_facts_db.facts ORDER BY random() LIMIT 1")
            .FirstAsync().ContinueWith(t => t.Result.ToResult());
    }


    public async Task<Result<AnimalFact>> getFactById(string id)
    {
        try
        {
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