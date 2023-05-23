using CleanArchitecture.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly EfDbContext _dbContext;
    public ICustomerRepository CustomerRepository { get; }
    public IProductRepository ProductRepository { get; }

    public UnitOfWork(EfDbContext dbContext,
        ICustomerRepository customerRepository,
        IProductRepository productRepository)
    {
        _dbContext = dbContext;
        CustomerRepository = customerRepository;
        ProductRepository = productRepository;
    }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}