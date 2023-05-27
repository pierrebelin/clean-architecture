namespace CleanArchitecture.Application.Mediator;

public class ValidationFailed : IDbResult
{
}

public class NotFound : IDbResult
{
}

public class NotSaved : IDbResult
{
}

public interface IDbResult
{

}