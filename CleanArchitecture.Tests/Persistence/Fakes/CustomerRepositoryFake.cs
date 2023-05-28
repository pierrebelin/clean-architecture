using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanArchitecture.Tests.Persistence.Fakes
{
    internal class CustomerRepositoryFake : ICustomerRepository
    {
        private readonly List<Customer> _customers = new();

        public void Add(Customer customerToAdd)
        {
            _customers.Add(customerToAdd);
        }

        public void Remove(Customer customerToDelete)
        {
            _customers.Remove(customerToDelete);
        }

        public Task<List<Customer>> GetAllAsync()
        {
            return Task.Run(() => _customers);
        }

        public Task<Customer?> GetByIdAsync(Guid id)
        {
            return Task.Run(() => _customers.FirstOrDefault(_ => _.Id == id));
        }

        public void Update(Customer customer)
        {
            var oldCustomer = _customers.FirstOrDefault(_ => _.Id == customer.Id);
            _customers.Remove(oldCustomer);
            _customers.Add(customer);
        }
    }
}
