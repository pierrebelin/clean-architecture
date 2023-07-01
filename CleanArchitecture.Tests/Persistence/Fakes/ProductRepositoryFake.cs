using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Tests.Persistence.Fakes.Database;

namespace CleanArchitecture.Tests.Persistence.Fakes
{
    internal class ProductRepositoryFake : IProductRepository
    {
        private readonly IDbFake _db;

        public ProductRepositoryFake(IDbFake db)
        {
            _db = db;
        }

        public void Add(Product productToAdd)
        {
            _db.Products.Add(new DbSetFake<Product>(productToAdd, DbStateFake.Added));
        }

        public Task<List<Product>> GetAllAsync()
        {
            return Task.FromResult(_db.Products.Select(_ => _.Element).ToList());
        }

        public Task<Product?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_db.Products.Select(_ => _.Element).FirstOrDefault(_ => _.Id == id));
        }
    }
}
