using System.Collections;
using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Tests.Persistence;
using CleanArchitecture.Tests.Persistence.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Tests.UnitTests.Customers.Data;

public class CustomersControllerTestData<T> : WebApplicationFactory<Program>, IEnumerable<object[]> where T : class, IUnitOfWorkFakeLoader
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder.ConfigureTestServices(services =>
        {
            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IUnitOfWork));
            services.Remove(serviceDescriptor);
            services.AddSingleton<IUnitOfWork, UnitOfWorkFake>();
            services.AddTransient<IProductRepository, ProductRepositoryFake>();
            services.AddTransient<ICustomerRepository, CustomerRepositoryFake>();
            services.AddTransient<IUnitOfWorkFakeLoader, T>();
        });
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            CreateClient(),
            Services.GetService<IUnitOfWork>()
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}