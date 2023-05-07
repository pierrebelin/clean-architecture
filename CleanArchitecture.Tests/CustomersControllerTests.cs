using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using CleanArchitecture.Domain.DomainObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Collections;
using CleanArchitecture.Domain.Persistence;
using System.Linq;
using System;
using CleanArchitecture.Tests.Fakes;
using static System.Net.Mime.MediaTypeNames;
using FluentAssertions.Common;

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
                var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IDataServiceFactory));
                services.Remove(serviceDescriptor);
                services.AddSingleton<IDataServiceFactory, DataServiceFactoryFake>();
            });
        }

        public void InitData()
        {
            var dataServiceFactory = Services.GetService<IDataServiceFactory>();
            var dataService = dataServiceFactory.CreateService<Customer>();
            dataService.Add(new Customer { Id = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d148"), Name = "Titouan" });
            dataService.Add(new Customer { Id = Guid.Parse("6709c6ff-b1e6-4c2e-a3dd-b1938623d149"), Name = "Michel" });
        }
    }
}