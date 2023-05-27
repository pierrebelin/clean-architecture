using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Core.Products.Queries;

public record GetProductQuery(Guid Id) : IRequest<Result<Product, NotFound>>;