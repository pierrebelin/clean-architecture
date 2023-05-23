using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanArchitecture.Tests.Fakes
{
    internal class CustomerRepositoryFake : ICustomerRepository
    {
        private readonly List<Customer> _customers = new();

        public void Add(Customer customerToAdd)
        {
            _customers.Add(customerToAdd);
        }

        public Task<List<Customer>> GetAllAsync()
        {
            return Task.Run(() => _customers);
        }

        public Task<Customer?> GetByIdAsync(Guid id)
        {
            return Task.Run(() => _customers.FirstOrDefault(_ => _.Id == id));
        }
    }
}
