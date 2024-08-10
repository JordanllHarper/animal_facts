using AnimalFactsApi.Model;
using FluentResults;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AnimalFactsApi.Dao;

public interface IFactDao
{
    Task<Result<AnimalFact?>> GetFact();
    Task<Result<AnimalFact?>> GetFactById(string id);
}

public class FactDao(IServiceProvider serviceProvider) : IFactDao
{
    // private readonly 
    public Task<Result<AnimalFact?>> GetFact()
    {
        using var context = serviceProvider.GetRequiredService<AnimalFactsContext>();
        return context.Facts.FromSqlInterpolated($"SELECT * FROM animal_facts_db.public.facts ORDER BY random() LIMIT 1")
            .FirstOrDefaultAsync().ContinueWith(t => t.Result.ToResult());
    }


    public Task<Result<AnimalFact?>> GetFactById(string id)
    {
        using var context = serviceProvider.GetRequiredService<AnimalFactsContext>();
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