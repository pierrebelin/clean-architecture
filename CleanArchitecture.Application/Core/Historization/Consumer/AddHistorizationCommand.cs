namespace CleanArchitecture.Application.Core.Historization.Consumer
{
    public record AddHistorizationCommand(Guid Id, object ObjectToHistorize);
}