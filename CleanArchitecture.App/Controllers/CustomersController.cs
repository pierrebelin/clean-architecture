using CleanArchitecture.Application;
using CleanArchitecture.Application.Customers.Commands;
using CleanArchitecture.Application.Customers.Queries;
using CleanArchitecture.Domain.DomainObjects;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.App.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator, ILogger<CustomersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(CancellationToken cancellationToken)
        {
            var result = await _mediator.SendRequest(new GetCustomersQuery(), cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.SendRequest(new GetCustomerQuery(id), cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }

        [HttpPost("")]
        public async Task<ActionResult<bool>> AddCustomer([FromBody] CreateCustomerCommand customer, CancellationToken cancellationToken)
        {
            var result = await _mediator.SendRequest(customer, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }
    }
}