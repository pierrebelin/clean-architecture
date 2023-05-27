using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Tests;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence;

public class UnitOfWorkFake : IUnitOfWork
{
    public ICustomerRepository CustomerRepository { get; }
    public IProductRepository ProductRepository { get; }

    public UnitOfWorkFake(IUnitOfWorkFakeLoader loader,
        ICustomerRepository customerRepository,
        IProductRepository productRepository)
    {
        CustomerRepository = customerRepository;
        ProductRepository = productRepository;

        loader.LoadInto(this);
    }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => 1, cancellationToken);
    }
}