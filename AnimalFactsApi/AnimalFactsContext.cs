using AnimalFactsApi.Model;
using Microsoft.EntityFrameworkCore;

namespace AnimalFactsApi;

public class AnimalFactsContext : DbContext
{
    public DbSet<AnimalFact> Facts { get; init; }
}