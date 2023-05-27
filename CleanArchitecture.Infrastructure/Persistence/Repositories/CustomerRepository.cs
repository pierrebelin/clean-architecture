using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanArchitecture.Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly EfDbContext _dbContext;

    public CustomerRepository(EfDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Customer?> GetByIdAsync(Guid id)
    {
        return _dbContext.Customers.FirstOrDefaultAsync(_ => _.Id == id);
    }

    public void Update(Customer customer)
    {
        _dbContext.Customers.Update(customer);
    }

    public void Remove(Customer customerToDelete)
    {
        _dbContext.Customers.Remove(customerToDelete);
    }

    public Task<List<Customer>> GetAllAsync()
    {
        return _dbContext.Customers.ToListAsync();
    }

    public void Add(Customer customerToAdd)
    {
        _dbContext.Customers.Add(customerToAdd);
    }
}