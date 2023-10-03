using CleanArchitecture.Application.Mediator.Dispatcher;
using MassTransit;
using STID.SMID.Application.Core.Dispatcher;

namespace CleanArchitecture.Application.Mediator.Filters.Utils;

public static class ConsumeContextExtension
{
    public static Task RespondWithError<TCommandType>(this ConsumeContext<TCommandType> context,IErrorReason reason) where TCommandType : class
    {
        return CreateResultInstance(context, reason);
    }

    private static async Task CreateResultInstance<T>(ConsumeContext<T> context, IErrorReason reason) where T : class
    {
        var type = typeof(T);
        var requestType = GetIRequestType(type);
        if (requestType is not null)
        {
            var typeGenericTypeArgument = requestType.GenericTypeArguments[0];
            var resultGenericType = typeof(Result<,>).MakeGenericType(typeGenericTypeArgument, typeof(IErrorReason));
            var instance = Activator.CreateInstance(resultGenericType, reason);
            await context.RespondAsync(instance);
        }
    }

    private static Type GetIRequestType(Type type)
    {
        foreach (var interfaceType in type.GetInterfaces())
        {
            if (interfaceType.GetGenericTypeDefinition().IsAssignableTo(typeof(ICommand<>)) || 
                interfaceType.GetGenericTypeDefinition().IsAssignableTo(typeof(IQuery<>)))
                return interfaceType;
        }

        return null;
    }
}