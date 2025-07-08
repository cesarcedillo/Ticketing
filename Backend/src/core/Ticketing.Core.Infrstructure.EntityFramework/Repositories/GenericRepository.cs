using Ticketing.Core.Domain.SeedWork.Interfaces;
using Ticketing.Core.Infrstructure.EntityFramework.Context;

namespace Ticketing.Core.Infrstructure.EntityFramework.Repositories;
public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IAggregateRoot, new()
{
  private readonly DbContextBase _dbContextBase;
  public IUnitOfWork UnitOfWork => _dbContextBase;

  public GenericRepository(DbContextBase dbContextBase)
  {
    _dbContextBase = dbContextBase;
  }

  public TEntity Add(TEntity entity)
  {
    var entityEntry = _dbContextBase.Add(entity);

    return entityEntry.Entity;
  }

  public void Delete(int id)
  {
    var entity = GetById(id);

    if (entity is not null)
    {
      _dbContextBase.Remove(entity);
    }
  }

  public IEnumerable<TEntity> GetAll()
  {
    return _dbContextBase.Set<TEntity>().ToList();
  }

  public TEntity? GetById(int id)
  {
    return _dbContextBase.Set<TEntity>().Find(id);
  }

  public TEntity Update(TEntity entity)
  {
    var entityEntry = _dbContextBase.Update(entity);

    return entityEntry.Entity;
  }
}
