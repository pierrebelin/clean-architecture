using System.Reflection;
using CleanArchitecture.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure;

public class EfDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Filename=db.sqlite");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}