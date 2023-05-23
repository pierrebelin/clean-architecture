using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Persistence
{
    public interface IProductRepository
    {
        void Add(Product productToAdd);
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
    }
}