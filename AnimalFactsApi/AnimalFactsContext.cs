using AnimalFactsApi.Model;
using Microsoft.EntityFrameworkCore;

namespace AnimalFactsApi;

public class AnimalFactsContext(DbContextOptions<AnimalFactsContext> options) : DbContext(options)
{
    public DbSet<AnimalFact> Facts { get; init; }
}