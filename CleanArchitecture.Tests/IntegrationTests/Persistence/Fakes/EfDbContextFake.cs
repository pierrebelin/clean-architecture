using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Tests.IntegrationTests.Persistence.Fakes;

public class EfDbContextFake : EfDbContext
{
    public EfDbContextFake(string connectionString) : base(connectionString)
    {
        Database.OpenConnection();
        Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString).EnableSensitiveDataLogging();
    }
}