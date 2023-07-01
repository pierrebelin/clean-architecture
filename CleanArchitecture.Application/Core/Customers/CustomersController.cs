using CleanArchitecture.Application.Core.Customers.Commands;
using CleanArchitecture.Application.Core.Customers.DTO;
using CleanArchitecture.Application.Core.Customers.Queries;
using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using MassTransit;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Core.Customers
{
    public static class CustomersController
    {
        public static IEndpointRouteBuilder AddCustomersMapEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("customers", CustomerHandlers.GetCustomers);
            endpoints.MapPost("customers", CustomerHandlers.AddCustomer);
            endpoints.MapGet("customers/{id}", CustomerHandlers.GetCustomer);
            endpoints.MapPut("customers/{id}", CustomerHandlers.UpdateCustomer);
            endpoints.MapDelete("customers/{id}", CustomerHandlers.DeleteCustomer);
            return endpoints;
        }
    }

    public static class CustomerHandlers
    {
        public static async Task<IResult> GetCustomers(IRequestClient<GetCustomersQuery> client, CancellationToken cancellationToken)
        {
            var response = await client.GetResponse<Result<IEnumerable<Customer>, NotFound>>(new GetCustomersQuery(), cancellationToken);
            return response.Message.Match(
                Results.Ok,
                Results.NotFound);
        }

        public static async Task<IResult> GetCustomer(Guid id, IRequestClient<GetCustomerQuery> client, CancellationToken cancellationToken)
        {
            var response = await client.GetResponse<Result<Customer, NotFound>>(new GetCustomerQuery(id), cancellationToken);
            return response.Message.Match(
                Results.Ok,
                Results.NotFound);

        }

        public static async Task<IResult> AddCustomer(CreateCustomerCommand customer, IRequestClient<CreateCustomerCommand> client, CancellationToken cancellationToken)
        {
            var response = await client.GetResponse<Result<bool, ValidationFailed>>(customer, cancellationToken);
            return response.Message.Match(
                m => Results.NoContent(),
                Results.BadRequest);
        }

        public static async Task<IResult> DeleteCustomer(Guid id, IRequestClient<DeleteCustomerCommand> client, CancellationToken cancellationToken)
        {
            var response = await client.GetResponse<Result<bool, IDbResult>>(new DeleteCustomerCommand(id), cancellationToken);
            return response.Message.Match(
                m => Results.NoContent(),
                Results.BadRequest);
        }

        public static async Task<IResult> UpdateCustomer(Guid id, UpdateCustomerRequest customer, IRequestClient<UpdateCustomerCommand> client, CancellationToken cancellationToken)
        {
            UpdateCustomerCommand command = new(id, customer.Name);
            var response = await client.GetResponse<Result<bool, IDbResult>>(command, cancellationToken);
            return response.Message.Match(
                m => Results.NoContent(),
                Results.NotFound);
        }
    }
}