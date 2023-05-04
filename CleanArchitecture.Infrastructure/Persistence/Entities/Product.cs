namespace CleanArchitecture.Infrastructure.Persistence.Entities;


public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
