namespace CleanArchitecture.Domain.Persistence
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}