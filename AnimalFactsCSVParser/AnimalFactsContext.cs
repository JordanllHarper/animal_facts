using AnimalFactsApi.Model;
using Microsoft.EntityFrameworkCore;

namespace AnimalFactsApi;

public class AnimalFactsContext(string connectionString) : DbContext
{
    public DbSet<AnimalFact> Facts { get; init; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(connectionString: connectionString);
}