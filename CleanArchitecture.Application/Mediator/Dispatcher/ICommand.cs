using CleanArchitecture.Application.Mediator.Dispatcher;

namespace STID.SMID.Application.Core.Dispatcher;

public interface ICommand<TOutput> : IRequest<TOutput> where TOutput : class
{
}
