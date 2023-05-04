using CleanArchitecture.Application.UseCases.Customers.Commands;
using CleanArchitecture.Application.UseCases.Customers.Queries;
using MassTransit;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.UseCases.Customers
{
    public static class CustomersController
    {
        public static IEndpointRouteBuilder AddCustomersMapEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("customers", CustomerHandlers.GetCustomers);
            endpoints.MapPost("customers", CustomerHandlers.AddCustomer);
            endpoints.MapGet("customers/{id}", CustomerHandlers.GetCustomer);
            return endpoints;
        }
    }

    public static class CustomerHandlers
    {
        public static async Task<IResult> GetCustomers(IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.SendRequest(new GetCustomersQuery(), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        }

        public static async Task<IResult> GetCustomer(Guid id, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.SendRequest(new GetCustomerQuery(id), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        }

        public static async Task<IResult> AddCustomer(CreateCustomerCommand customer, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.SendRequest(customer, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        }
    }
}