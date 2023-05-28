using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;

namespace CleanArchitecture.Tests.Persistence.Fakes
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
