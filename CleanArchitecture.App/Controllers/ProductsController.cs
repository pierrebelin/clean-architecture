using CleanArchitecture.Application.Products.Commands;
using CleanArchitecture.Application.Products.Queries;
using CleanArchitecture.Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.App.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ISender _mediator;

        public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductsQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductQuery(id), cancellationToken);
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpPost("")]
        public async Task<ActionResult> AddProduct([FromBody] CreateProductCommand product, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(product, cancellationToken);
            return result.IsSuccess ? Ok() : BadRequest();
        }
    }
}