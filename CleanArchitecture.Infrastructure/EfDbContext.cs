using System.Reflection;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure;

public class EfDbContext : DbContext, IEfDbContext
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

        builder.Entity<Product>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(64);

            cfg.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(128);

            cfg.HasIndex(e => e.Name)
                .IsUnique();
        });
    }
}