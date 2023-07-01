using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Tests.Persistence.Fakes.Database;
using System.Security.Cryptography;
using static Dapper.SqlMapper;

namespace CleanArchitecture.Tests.Persistence.Fakes
{
    internal class CustomerRepositoryFake : ICustomerRepository
    {
        private readonly IDbFake _db;

        public CustomerRepositoryFake(IDbFake db)
        {
            _db = db;
        }

        public void Add(Customer customerToAdd)
        {
            _db.Add(customerToAdd);
        }

        public void Remove(Customer customerToDelete)
        {
            _db.Remove(customerToDelete);
        }

        public Task<List<Customer>> GetAllAsync()
        {
            return Task.FromResult(_db.Customers.Select(_ => _.Element).ToList());
        }

        public Task<Customer?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_db.Customers.Select(_ => _.Element).FirstOrDefault(_ => _.Id == id));
        }

        public void Update(Customer customer)
        {
            _db.Update(customer);
        }
    }
}
