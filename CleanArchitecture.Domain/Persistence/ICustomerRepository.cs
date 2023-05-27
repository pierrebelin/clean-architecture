using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Persistence
{
    public interface ICustomerRepository
    {
        void Add(Customer customerToAdd);
        void Remove(Customer customerToDelete);
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(Guid id);
        void Update(Customer customer);
    }
}