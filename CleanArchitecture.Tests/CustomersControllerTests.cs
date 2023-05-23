using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Tests.Fakes;

namespace CleanArchitecture.Tests
{
    public class CustomersControllerTests : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CustomersControllerTests(ApiWebApplicationFactory application)
        {
            _client = application.CreateClient();
            application.InitData();
        }

        [Theory]
        [InlineData("/customers")]
        [InlineData("/customers/6709c6ff-b1e6-4c2e-a3dd-b1938623d148")]
        public async Task ShouldReturns200_WhenAskingForExistingGetEndpoints(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ShouldReturnsCustomers_WhenAskingForCustomerList()
        {
            var response = await _client.GetAsync("/customers");

            var responseContent = await response.Content.ReadAsStringAsync();
            var customers = JsonSerializer.Deserialize<IEnumerable<Customer>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            customers.Should().HaveCount(2);
        }
    }

    public class ApiWebApplicationFactory : WebApplicationFactory<Program>
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
            });
        }

        public void InitData()
        {
            var unitOfWork = Services.GetService<IUnitOfWork>();
            unitOfWork.CustomerRepository.Add(new Customer { Id = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d148"), Name = "Titouan" });
            unitOfWork.CustomerRepository.Add(new Customer { Id = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d149"), Name = "Michel" });
        }
    }
}