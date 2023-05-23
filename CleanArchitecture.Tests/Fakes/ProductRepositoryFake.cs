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
    internal class ProductRepositoryFake : IProductRepository
    {
        private readonly List<Product> _products = new();


        public void Add(Product productToAdd)
        {
            _products.Add(productToAdd);
        }

        public Task<List<Product>> GetAllAsync()
        {
            return Task.Run(() => _products);
        }

        public Task<Product?> GetByIdAsync(Guid id)
        {
            return Task.Run(() => _products.FirstOrDefault(_ => _.Id == id));
        }
    }
}
