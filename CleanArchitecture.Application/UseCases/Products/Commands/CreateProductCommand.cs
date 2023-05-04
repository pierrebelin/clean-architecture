using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.DomainObjects;
using MediatR;

namespace CleanArchitecture.Application.UseCases.Products.Commands;

public record CreateProductCommand(string Name) : IRequest<Result<bool>>;

//public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result> where TCommand: ICommand { }
//public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand: ICommand<TResponse> { }

//public interface ICommand : IRequest<Result> { }
//public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }


//public class Result
//{
//    public bool IsSuccess { get; set; }
//    public Exception Error { get; set; }

//    public static Result Success()
//    {
//        throw new NotImplementedException();
//    }
//}

//public class Result<T> { }