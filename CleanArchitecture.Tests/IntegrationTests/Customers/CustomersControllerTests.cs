using System.Net;
using System.Text;
using System.Text.Json;
using CleanArchitecture.Application.Core.Customers.CreateCustomer;
using CleanArchitecture.Application.Core.Customers.GetCustomers;
using CleanArchitecture.Application.Core.Customers.UpdateCustomer;
using CleanArchitecture.Tests.IntegrationTests.Customers.Data;
using FluentAssertions;

namespace CleanArchitecture.Tests.IntegrationTests.Customers
{
    public class CustomersControllerTests
    {
        private readonly CustomersFixture _fixture = new CustomersFixture();

        [Fact]
        public async Task ShouldReturnsCustomers_WhenAskingForCustomerList()
        {
            var fixture = _fixture
                .AddCustomer(Guid.NewGuid(), "name1")
                .AddCustomer(Guid.NewGuid(), "name2")
                .SaveInDatabase();
            
            var response = await fixture.Client.GetAsync("/customers");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            var customers = JsonSerializer.Deserialize<GetCustomerResults>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            customers.Customers.Should().HaveCount(2);
        }
        
        [Fact]
        public async Task ShouldAddCustomer_WhenAskingToRouteCustomersPost()
        {
            var fixture = _fixture
                .AddCustomer(Guid.NewGuid(), "name1")
                .AddCustomer(Guid.NewGuid(), "name2")
                .SaveInDatabase();
            
            var resultss = await fixture.CustomerRepository.GetAllAsync();
            var customerName = "Thierry";
            CreateCustomerCommand newCustomer = new(customerName);
            var content = CreateHttpContent(newCustomer);
            
            var response = await fixture.Client.PostAsync("/customers", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var results = await fixture.CustomerRepository.GetAllAsync();
            results.Should().HaveCount(3);
        }
        
        [Fact]
        public async Task ShouldUpdateCustomer_WhenAskingToRouteCustomerPut()
        {
            var customerToUpdateGuid = Guid.NewGuid();
            var fixture = _fixture
                .AddCustomer(customerToUpdateGuid, "name1")
                .AddCustomer(Guid.NewGuid(), "name2")
                .SaveInDatabase();
            
            UpdateCustomerRequest customerRequest = new() { Name = "newname" };
            var content = CreateHttpContent(customerRequest);
            var response = await fixture.Client.PutAsync($"/customers/{customerToUpdateGuid}", content);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            
            var allCustomers = await fixture.CustomerRepository.GetAllAsync();
            var updatedCustomer = allCustomers.FirstOrDefault(c => c.Id == customerToUpdateGuid);
            updatedCustomer.Should().NotBeNull();
            updatedCustomer?.Name.Should().Be(customerRequest.Name);
        }

        // [Theory]
        // [ClassData(typeof(CustomersTestWebApplication<CustomersTestData>))]
        // public async Task ShouldRemoveCustomer_WhenAskingToRouteCustomerDelete(HttpClient client, ICustomerRepository customerRepository)
        // {
        //     Guid guidToRemove = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d148");
        //
        //     var response = await client.DeleteAsync($"/customers/{guidToRemove}");
        //
        //     response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        //     var allCustomers = await customerRepository.GetAllAsync();
        //     allCustomers.Should().HaveCount(1);
        // }

        private StringContent CreateHttpContent(object objectToSend)
        {
            var jsonString = JsonSerializer.Serialize(objectToSend);
            StringContent content = new(jsonString, Encoding.UTF8, "application/json");
            return content;
        }
    }
}