using System.Net;
using System.Text;
using System.Text.Json;
using CleanArchitecture.Application.Core.Customers.Commands;
using CleanArchitecture.Application.Core.Customers.DTO;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Infrastructure.Persistence.Repositories;
using CleanArchitecture.Tests.Persistence.Data;
using CleanArchitecture.Tests.UnitTests.Customers.Data;
using FluentAssertions;

namespace CleanArchitecture.Tests.UnitTests.Customers
{
    public class CustomersControllerTests
    {
        public CustomersControllerTests()
        {
        }

        [Theory]
        [ClassData(typeof(CustomersTestWebApplication<CustomersTestData>))]
        public async Task ShouldReturnsCustomers_WhenAskingForCustomerList(HttpClient client, ICustomerRepository customerRepository)
        {
            var response = await client.GetAsync("/customers");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            var customers = JsonSerializer.Deserialize<IEnumerable<Customer>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            customers.Should().HaveCount(2);
        }

        [Theory]
        [ClassData(typeof(CustomersTestWebApplication<CustomersTestData>))]
        public async Task ShouldAddCustomer_WhenAskingToRouteCustomersPost(HttpClient client, ICustomerRepository customerRepository)
        {
            var customerName = "Thierry";
            CreateCustomerCommand newCustomer = new(customerName);
            StringContent content = CreateHttpContent(newCustomer);

            var response = await client.PostAsync("/customers", content);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var allCustomers = await customerRepository.GetAllAsync();
            allCustomers.Should().HaveCount(3);
        }
        
        [Theory]
        [ClassData(typeof(CustomersTestWebApplication<CustomersTestData>))]
        public async Task ShouldRemoveCustomer_WhenAskingToRouteCustomerDelete(HttpClient client, ICustomerRepository customerRepository)
        {
            Guid guidToRemove = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d148");

            var response = await client.DeleteAsync($"/customers/{guidToRemove}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var allCustomers = await customerRepository.GetAllAsync();
            allCustomers.Should().HaveCount(1);
        }

        [Theory]
        [ClassData(typeof(CustomersTestWebApplication<CustomersTestData>))]
        public async Task ShouldUpdateCustomer_WhenAskingToRouteCustomerPut(HttpClient client, ICustomerRepository customerRepository)
        {
            //var uow = _serviceProvider.GetService<IUnitOfWork>();
            Guid guidToUpdate = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d148");
            UpdateCustomerRequest customerRequest = new() { Name = "Jean" };
            StringContent content = CreateHttpContent(customerRequest);

            var response = await client.PutAsync($"/customers/{guidToUpdate}", content);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var allCustomers = await customerRepository.GetAllAsync();
            allCustomers.Should().HaveCount(2);
            var updatedCustomer = allCustomers.FirstOrDefault(_ => _.Id == guidToUpdate);
            updatedCustomer.Should().NotBeNull();
            updatedCustomer?.Name.Should().Be(customerRequest.Name);
        }

        private StringContent CreateHttpContent(object objectToSend)
        {
            var jsonString = JsonSerializer.Serialize(objectToSend);
            StringContent content = new(jsonString, Encoding.UTF8, "application/json");
            return content;
        }
    }
}