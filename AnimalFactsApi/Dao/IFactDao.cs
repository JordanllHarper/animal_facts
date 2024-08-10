using AnimalFactsApi.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AnimalFactsApi.Dao;

public interface IFactDao
{
    Task<Result<AnimalFact?>> GetFact();
    Task<Result<AnimalFact?>> GetFactById(string id);
}

public class FactDao(AnimalFactsContext context) : IFactDao
{
    public Task<Result<AnimalFact?>> GetFact() =>
        context.Facts.FromSqlInterpolated($"SELECT * FROM animal_facts_db.facts ORDER BY random() LIMIT 1")
            .FirstOrDefaultAsync().ContinueWith(t => t.Result.ToResult());


    public Task<Result<AnimalFact?>> GetFactById(string id)
    {
        try
        {
            var parsed = int.Parse(id);
            return context.Facts.Where(item => item.Id == parsed).FirstOrDefaultAsync()
                .ContinueWith(item => item.Result.ToResult());
        }
        catch (FormatException)
        {
            return Task.FromResult<Result<AnimalFact?>>(Result.Fail("The id supplied wasn't parseable to an integer."));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Task.FromResult<Result<AnimalFact?>>(Result.Fail("Something went wrong"));
        }
    }
}