namespace CleanArchitecture.Application.Mediator;

public class Result<T>
{
    public bool IsSuccess => Value is not null;
    public required T? Value { get; set; }
}