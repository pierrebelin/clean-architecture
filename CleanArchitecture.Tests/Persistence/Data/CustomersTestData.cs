using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Tests.Persistence.Fakes;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Tests.Persistence.Data;

internal class CustomersTestData : IUnitOfWorkFakeLoader
{
    public bool IsLoaded { get; private set; }

    public void LoadInto(UnitOfWorkFake unitOfWork, IServiceProvider serviceProvider)
    {
        if (IsLoaded)
        {
            return;
        }

        var customerRepository = serviceProvider.GetService<ICustomerRepository>();

        if (customerRepository == null)
        {
            throw new Exception();
        }

        customerRepository.Add(new Customer { Id = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d148"), Name = "Titouan" });
        customerRepository.Add(new Customer { Id = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d149"), Name = "Michel" });

        unitOfWork.SaveChangesAsync(default);
        IsLoaded = true;
    }
}
