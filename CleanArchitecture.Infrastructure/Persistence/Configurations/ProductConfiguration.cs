using CleanArchitecture.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(64);

        builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(128);

        builder.HasIndex(e => e.Name)
                .IsUnique();
    }
}