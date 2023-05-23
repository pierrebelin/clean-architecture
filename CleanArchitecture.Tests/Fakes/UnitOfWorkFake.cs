using CleanArchitecture.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence;

public class UnitOfWorkFake : IUnitOfWork
{
    public ICustomerRepository CustomerRepository { get; }
    public IProductRepository ProductRepository { get; }

    public UnitOfWorkFake(ICustomerRepository customerRepository,
        IProductRepository productRepository)
    {
        CustomerRepository = customerRepository;
        ProductRepository = productRepository;
    }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => 1, cancellationToken);
    }
}