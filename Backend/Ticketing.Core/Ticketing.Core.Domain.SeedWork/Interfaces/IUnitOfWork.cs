namespace Ticketing.Core.Domain.SeedWork.Interfaces;
public interface IUnitOfWork : IDisposable
{
  Task BeginTransactionAsync(CancellationToken cancellationToken = default);
  Task CommitAsync(CancellationToken cancellationToken = default);
  Task RollbackAsync(CancellationToken cancellationToken = default);


  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
  void ClearChangeTracker();
}
