namespace Ticketing.Core.Domain.SeedWork.Interfaces;

public interface IGenericRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : IAggregateRoot
{
  TEntity? GetById(int id);
  Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
  IEnumerable<TEntity> GetAll();
  Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
  TEntity Add(TEntity entity);
  Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
  TEntity Update(TEntity entity);
  Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
  void Delete(int id);
  Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
