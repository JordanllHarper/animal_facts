using AnimalFactsApi.Dao;
using AnimalFactsApi.Model;
using FluentResults;

namespace AnimalFactsApi;

public interface IFactRepository
{
    Task<Result<AnimalFact>> getFact(string? id);
}

class FactRepository(IFactDao dao) : IFactRepository
{
    public async Task<Result<AnimalFact>> getFact(string? id) =>
        await (id == null ? dao.getFact() : dao.getFactById(id));
}