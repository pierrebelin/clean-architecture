using CleanArchitecture.Application.Core.Customers.Commands;
using CleanArchitecture.Application.Core.Customers.DTO;
using CleanArchitecture.Application.Core.Customers.Queries;
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
        public static async Task<IResult> GetCustomers(IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.SendRequest(new GetCustomersQuery(), cancellationToken);
            return result.Match<IResult>(
                m => Results.Ok(m),
                failed => Results.NotFound());
        }

        public static async Task<IResult> GetCustomer(Guid id, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.SendRequest(new GetCustomerQuery(id), cancellationToken);
            return result.Match<IResult>(
                m => Results.Ok(m),
                failed => Results.NotFound());
        }

        public static async Task<IResult> AddCustomer(CreateCustomerCommand customer, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.SendRequest(customer, cancellationToken);
            return result.Match<IResult>(
                m => Results.NoContent(),
                failed => Results.BadRequest());
        }

        public static async Task<IResult> DeleteCustomer(Guid id, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.SendRequest(new DeleteCustomerCommand(id), cancellationToken);
            return result.Match<IResult>(
                m => Results.NoContent(),
                failed => Results.NotFound());
        }

        public static async Task<IResult> UpdateCustomer(Guid id, UpdateCustomerRequest customer, IMediator mediator, CancellationToken cancellationToken)

        {
            UpdateCustomerCommand command = new(id, customer.Name);
            var result = await mediator.SendRequest(command, cancellationToken);
            return result.Match<IResult>(
                m => Results.NoContent(),
                failed => Results.NotFound());
        }
    }
}