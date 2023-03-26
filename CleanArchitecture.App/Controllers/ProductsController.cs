using CleanArchitecture.Application.Products.Commands;
using CleanArchitecture.Application.Products.Queries;
using CleanArchitecture.Domain.Entities;
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

        [HttpPost("")]
        public async Task<ActionResult> AddProduct([FromBody] CreateProductCommand product, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(product, cancellationToken);
            return result.IsSuccess ? Ok() : BadRequest();
        }
    }
}