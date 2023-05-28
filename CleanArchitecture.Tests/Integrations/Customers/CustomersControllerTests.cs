using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Application.Core.Customers.Commands;
using System;
using System.Collections;
using CleanArchitecture.Application.Core.Customers.DTO;
using FluentAssertions.Common;
using System.Threading.Channels;
using CleanArchitecture.Tests.Persistence.Fakes;
using CleanArchitecture.Tests.Integrations.Customers.Data;
using CleanArchitecture.Tests.Persistence.Data;

namespace CleanArchitecture.Tests.Integrations.Customers
{
    public class CustomersControllerTests
    {
        public CustomersControllerTests()
        {
        }

        [Theory]
        [ClassData(typeof(CustomersControllerTestData<UnitOfWorkFakeCustomerControllerLoader>))]
        public async Task ShouldReturnsCustomers_WhenAskingForCustomerList(HttpClient client, IUnitOfWork unitOfWork)
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
        [ClassData(typeof(CustomersControllerTestData<UnitOfWorkFakeCustomerControllerLoader>))]
        public async Task ShouldAddCustomer_WhenAskingToRouteCustomersPost(HttpClient client, IUnitOfWork unitOfWork)
        {
            var customerName = "Thierry";
            CreateCustomerCommand newCustomer = new(customerName);
            StringContent content = CreateHttpContent(newCustomer);

            var response = await client.PostAsync("/customers", content);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var allCustomers = await unitOfWork.CustomerRepository.GetAllAsync();
            allCustomers.Should().HaveCount(3);
        }

        [Theory]
        [ClassData(typeof(CustomersControllerTestData<UnitOfWorkFakeCustomerControllerLoader>))]
        public async Task ShouldRemoveCustomer_WhenAskingToRouteCustomerDelete(HttpClient client, IUnitOfWork unitOfWork)
        {
            Guid guidToRemove = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d148");

            var response = await client.DeleteAsync($"/customers/{guidToRemove}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var allCustomers = await unitOfWork.CustomerRepository.GetAllAsync();
            allCustomers.Should().HaveCount(1);
        }

        [Theory]
        [ClassData(typeof(CustomersControllerTestData<UnitOfWorkFakeCustomerControllerLoader>))]
        public async Task ShouldUpdateCustomer_WhenAskingToRouteCustomerPut(HttpClient client, IUnitOfWork unitOfWork)
        {
            //var uow = _serviceProvider.GetService<IUnitOfWork>();
            Guid guidToUpdate = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d148");
            UpdateCustomerRequest customerRequest = new() { Name = "Jean" };
            StringContent content = CreateHttpContent(customerRequest);

            var response = await client.PutAsync($"/customers/{guidToUpdate}", content);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var allCustomers = await unitOfWork.CustomerRepository.GetAllAsync();
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
}