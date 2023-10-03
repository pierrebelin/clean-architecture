using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Tests.IntegrationTests.Customers.Data;

internal class CustomersFixture
{
    private readonly IUnitOfWork _unitOfWork;
    public ICustomerRepository CustomerRepository { get; }
    public HttpClient Client { get; }
    public IServiceProvider ServiceProvider { get; }
    
    public CustomersFixture()
    {
        var webApplication = new CustomersTestWebApplication();
        var serviceScope = webApplication.Services.CreateScope();

        ServiceProvider = serviceScope.ServiceProvider;

        Client = webApplication.CreateClient();
        Client.BaseAddress = new Uri("https://localhost:7223");

        CustomerRepository = ServiceProvider.GetService<ICustomerRepository>() ?? throw new Exception();
        _unitOfWork = ServiceProvider.GetService<IUnitOfWork>() ?? throw new Exception();
    }
    
    public CustomersFixture AddCustomer(Guid id, string name)
    {
        var customer = new Customer { Id = id, Name = name };
        CustomerRepository.Add(customer);
        return this;
    }

    public CustomersFixture SaveInDatabase()
    {
        _unitOfWork.SaveChangesAsync(default);
        return this;
    }
}
