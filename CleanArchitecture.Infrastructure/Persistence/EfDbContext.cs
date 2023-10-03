using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence;

public class EfDbContext : DbContext
{
    protected readonly string _connectionString;
    public virtual DbSet<Customer> Customers { get; set; }

    public EfDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}