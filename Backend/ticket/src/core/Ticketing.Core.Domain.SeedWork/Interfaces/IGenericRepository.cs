namespace Ticketing.Core.Domain.SeedWork.Interfaces;

public interface IGenericRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : IAggregateRoot
{
  TEntity? GetById(int id);
  IEnumerable<TEntity> GetAll();
  TEntity Add(TEntity entity);
  TEntity Update(TEntity entity);
  void Delete(int id);
}
