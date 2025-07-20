namespace Ticketing.Core.Domain.SeedWork.Interfaces;

public interface IBaseRepository<T> where T : IAggregateRoot
{
  IUnitOfWork UnitOfWork { get; }
}
