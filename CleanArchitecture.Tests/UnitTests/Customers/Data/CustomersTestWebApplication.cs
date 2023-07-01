using System.Collections;
using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Tests.Persistence;
using CleanArchitecture.Tests.Persistence.Fakes;
using CleanArchitecture.Tests.Persistence.Fakes.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Tests.UnitTests.Customers.Data;

internal class CustomersTestWebApplication<T> : WebApplicationFactory<Program>, IEnumerable<object[]> where T : class, IUnitOfWorkFakeLoader
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder.ConfigureTestServices(services =>
        {
            //var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IUnitOfWork));
            //if (serviceDescriptor != null)
            //{
            //    services.Remove(serviceDescriptor);
            //}

            services.AddSingleton<IDbFake, DbFake>();

            services.AddTransient<IProductRepository, ProductRepositoryFake>();
            services.AddTransient<ICustomerRepository, CustomerRepositoryFake>();
            services.AddTransient<IUnitOfWork, UnitOfWorkFake>();
            services.AddSingleton<IUnitOfWorkFakeLoader, T>();
        });
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        var scope = Services.CreateScope();
        var customerRepository = scope.ServiceProvider.GetService<ICustomerRepository>();
        // Call this to be sure UnitOfWorkFake is loaded
        var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

        yield return new object[]
        {
            CreateClient(),
            customerRepository
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}