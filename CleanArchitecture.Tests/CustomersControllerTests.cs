using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Tests.Fakes;
using CleanArchitecture.Application.Core.Customers.Commands;
using System;
using CleanArchitecture.Application.Core.Customers.DTO;
using FluentAssertions.Common;

namespace CleanArchitecture.Tests
{
    public class CustomersControllerTests : IClassFixture<ApiWebApplicationFactory<UnitOfWorkFakeCustomerControllerLoader>>
    {
        private readonly HttpClient _client;
        private readonly IServiceProvider _serviceProvider;

        public CustomersControllerTests(ApiWebApplicationFactory<UnitOfWorkFakeCustomerControllerLoader> application)
        {
            _client = application.CreateClient();
            _serviceProvider = application.Services;
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

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            var customers = JsonSerializer.Deserialize<IEnumerable<Customer>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            customers.Should().HaveCount(2);
        }

        [Fact]
        public async Task ShouldAddCustomer_WhenAskingToRouteCustomersPost()
        {
            var uow = _serviceProvider.GetService<IUnitOfWork>();
            var customerName = "Thierry";
            CreateCustomerCommand newCustomer = new(customerName);
            StringContent content = CreateHttpContent(newCustomer);

            var response = await _client.PostAsync("/customers", content);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var allCustomers = await uow.CustomerRepository.GetAllAsync();
            allCustomers.Should().HaveCount(3);
        }

        [Fact]
        public async Task ShouldRemoveCustomer_WhenAskingToRouteCustomerDelete()
        {
            var uow = _serviceProvider.GetService<IUnitOfWork>();
            Guid guidToRemove = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d148");

            var response = await _client.DeleteAsync($"/customers/{guidToRemove}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var allCustomers = await uow.CustomerRepository.GetAllAsync();
            allCustomers.Should().HaveCount(1);
        }

        [Fact]
        public async Task ShouldUpdateCustomer_WhenAskingToRouteCustomerPut()
        {
            var uow = _serviceProvider.GetService<IUnitOfWork>();
            Guid guidToUpdate = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d148");
            UpdateCustomerRequest customerRequest = new() {Name = "Jean"};
            StringContent content = CreateHttpContent(customerRequest);

            var response = await _client.PutAsync($"/customers/{guidToUpdate}", content);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var allCustomers = await uow.CustomerRepository.GetAllAsync();
            allCustomers.Should().HaveCount(2);
            var updatedCustomer = allCustomers.FirstOrDefault(_ => _.Id == guidToUpdate);
            updatedCustomer.Should().NotBeNull();
            updatedCustomer.Name.Should().Be(customerRequest.Name);
        }

        private StringContent CreateHttpContent(object objectToSend)
        {
            var jsonString = JsonSerializer.Serialize(objectToSend);
            StringContent content = new(jsonString, Encoding.UTF8, "application/json");
            return content;
        }
    }

    public class ApiWebApplicationFactory<T> : WebApplicationFactory<Program> where T : class, IUnitOfWorkFakeLoader
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
    }

    public class UnitOfWorkFakeCustomerControllerLoader : IUnitOfWorkFakeLoader
    {
        public void LoadInto(UnitOfWorkFake unitOfWork)
        {
            unitOfWork.CustomerRepository.Add(new Customer { Id = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d148"), Name = "Titouan" });
            unitOfWork.CustomerRepository.Add(new Customer { Id = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d149"), Name = "Michel" });
        }
    }

    public interface IUnitOfWorkFakeLoader
    {
        void LoadInto(UnitOfWorkFake unitOfWork);
    }
}