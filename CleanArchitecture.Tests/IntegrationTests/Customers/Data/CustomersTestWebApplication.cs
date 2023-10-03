using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Tests.IntegrationTests.Persistence.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanArchitecture.Tests.IntegrationTests.Customers.Data;

internal class CustomersTestWebApplication : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = "DataSource=myshareddb;mode=memory;cache=shared";
        _ = builder.ConfigureTestServices(services =>
        {
            services.AddScoped<EfDbContext>(_ => new EfDbContextFake(connectionString));
        });
    }
}