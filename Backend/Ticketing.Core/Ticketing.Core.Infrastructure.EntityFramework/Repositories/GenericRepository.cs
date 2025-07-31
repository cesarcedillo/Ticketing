using Microsoft.EntityFrameworkCore;
using System.Threading;
using Ticketing.Core.Domain.SeedWork.Interfaces;
using Ticketing.Core.Infrastructure.EntityFramework.Context;

namespace Ticketing.Core.Infrastructure.EntityFramework.Repositories;
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

  public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
  {
    var entityEntry = await _dbContextBase.AddAsync(entity, cancellationToken);

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

  public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
  {
    var entity = await GetByIdAsync(id, cancellationToken);

    if (entity is not null)
    {
      _dbContextBase.Remove(entity);
    }
  }

  public IEnumerable<TEntity> GetAll()
  {
    return _dbContextBase.Set<TEntity>().ToList();
  }
  public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    return await _dbContextBase.Set<TEntity>().ToListAsync(cancellationToken);
  }

  public TEntity? GetById(int id)
  {
    return _dbContextBase.Set<TEntity>().Find(id);
  }

  public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
  {
    return await _dbContextBase.Set<TEntity>().FindAsync(id, cancellationToken);
  }

  public TEntity Update(TEntity entity)
  {
    var entityEntry = _dbContextBase.Update(entity);

    return entityEntry.Entity;
  }

  public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
  {
    var entityEntry = _dbContextBase.Update(entity);

    return Task.FromResult(entityEntry.Entity);
  }
}
