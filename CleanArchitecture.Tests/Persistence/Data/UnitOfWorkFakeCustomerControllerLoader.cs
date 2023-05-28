using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Tests.Persistence.Fakes;

namespace CleanArchitecture.Tests.Persistence.Data;

public class UnitOfWorkFakeCustomerControllerLoader : IUnitOfWorkFakeLoader
{
    public void LoadInto(UnitOfWorkFake unitOfWork)
    {
        unitOfWork.CustomerRepository.Add(new Customer { Id = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d148"), Name = "Titouan" });
        unitOfWork.CustomerRepository.Add(new Customer { Id = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d149"), Name = "Michel" });
    }
}
