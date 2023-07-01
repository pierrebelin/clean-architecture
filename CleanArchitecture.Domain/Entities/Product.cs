using CleanArchitecture.Domain.Persistence;

namespace CleanArchitecture.Domain.Entities;


public class Product : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

