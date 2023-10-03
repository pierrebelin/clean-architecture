using CleanArchitecture.Application.Core.Customers.CreateCustomer;
using CleanArchitecture.Application.Core.Customers.DeleteCustomer;
using CleanArchitecture.Application.Core.Customers.GetCustomer;
using CleanArchitecture.Application.Core.Customers.GetCustomers;
using CleanArchitecture.Application.Core.Customers.UpdateCustomer;
using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Application.Mediator.Dispatcher;
using CleanArchitecture.Domain.Entities;
using MassTransit;

namespace CleanArchitecture.Application.Core.Customers
{
    public static class CustomersEndpoints
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
        public static async Task<IResult> GetCustomers(IDispatcher dispatcher, CancellationToken cancellationToken)
        {
            var response = await dispatcher.Send(new GetCustomersQuery(), cancellationToken);
            return response.Match(
                Results.Ok,
                Results.NotFound);
        }

        public static async Task<IResult> GetCustomer(Guid id, IDispatcher dispatcher, CancellationToken cancellationToken)
        {
            var response = await dispatcher.Send(new GetCustomerQuery(id), cancellationToken);
            return response.Match(
                Results.Ok,
                Results.NotFound);

        }

        public static async Task<IResult> AddCustomer(CreateCustomerCommand customer, IDispatcher dispatcher, CancellationToken cancellationToken)
        {
            var response = await dispatcher.Send(customer, cancellationToken);
            return response.Match(
                m => Results.NoContent(),
                Results.BadRequest);
        }

        public static async Task<IResult> DeleteCustomer(Guid id, IDispatcher dispatcher, CancellationToken cancellationToken)
        {
            //var response = await client.GetResponse<Result<bool, IDbResult>>(new DeleteCustomerCommand(id), cancellationToken);
            // var response = await dispatcher.Send(new StartCustomerDeletionEvent(id), cancellationToken);
            return null;

            //var response = await client.GetResponse<Result<bool, IDbResult>>(new DeleteCustomerCommand(id), cancellationToken);
            //return response.Message.Match(
            //    m => Results.NoContent(),
            //    Results.BadRequest);
        }

        public static async Task<IResult> UpdateCustomer(Guid id, UpdateCustomerRequest customer, IDispatcher dispatcher, CancellationToken cancellationToken)
        {
            UpdateCustomerCommand command = new(id, customer.Name);
            var response = await dispatcher.Send(command, cancellationToken);
            return response.Match(
                m => Results.NoContent(),
                Results.NotFound);
        }
    }
}