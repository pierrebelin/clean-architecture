using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Persistence
{
    public interface ICustomerRepository
    {
        void Add(Customer customerToAdd);
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(Guid id);
    }
}