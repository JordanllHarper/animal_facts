using AnimalFactsApi.Model;
using Microsoft.EntityFrameworkCore;

namespace AnimalFactsCSVParser;

public class AnimalFactsContext(string connectionString) : DbContext
{
    public DbSet<AnimalFact> Facts { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.Entity<AnimalFact>().Property(b => b.Id).UseIdentityAlwaysColumn();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
        .UseNpgsql(connectionString: connectionString).UseSnakeCaseNamingConvention();
}