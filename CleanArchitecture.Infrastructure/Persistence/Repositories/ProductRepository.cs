using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly EfDbContext _dbContext;
    public ProductRepository(EfDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Product?> GetByIdAsync(Guid id)
    {
        return _dbContext.Products.FirstOrDefaultAsync(_ => _.Id == id);
    }

    public Task<List<Product>> GetAllAsync()
    {
        return _dbContext.Products.ToListAsync();
    }

    public void Add(Product productToAdd)
    {
        _dbContext.Products.Add(productToAdd);
    }
}