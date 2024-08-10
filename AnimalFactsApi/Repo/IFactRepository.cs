using AnimalFactsApi.Dao;
using AnimalFactsApi.Model;
using FluentResults;

namespace AnimalFactsApi.Repo;

public interface IFactRepository
{
    Task<Result<AnimalFact?>> GetFact(string? id);
}

class FactRepository(IFactDao dao) : IFactRepository
{
    public Task<Result<AnimalFact?>> GetFact(string? id) => id == null ? dao.GetFact() : dao.GetFactById(id);
}