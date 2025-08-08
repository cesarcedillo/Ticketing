using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ticketing.Core.Domain.SeedWork.Interfaces;

namespace Ticketing.Core.Infrastructure.EntityFramework.Context;
public class DbContextBase : DbContext, IUnitOfWork, IAsyncDisposable
{

  private IDbContextTransaction? _currentTransaction;

  public DbContextBase() { }

  public DbContextBase(DbContextOptions options) : base(options)
  {
  }

  public virtual async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
  {
    await SaveChangesAsync(cancellationToken);
    return true;
  }

  public void ClearChangeTracker()
  {
    ChangeTracker?.Clear();
  }

  public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
  {
    if (_currentTransaction != null)
      return;

    _currentTransaction = await this.Database.BeginTransactionAsync(cancellationToken);
  }

  public async Task CommitAsync(CancellationToken cancellationToken = default)
  {
    try
    {
      await base.SaveChangesAsync(cancellationToken);
      if (_currentTransaction != null)
        await _currentTransaction.CommitAsync(cancellationToken);
    }
    finally
    {
      await DisposeTransactionAsync();
    }
  }

  public async Task RollbackAsync(CancellationToken cancellationToken = default)
  {
    try
    {
      if (_currentTransaction != null)
        await _currentTransaction.RollbackAsync(cancellationToken);
    }
    finally
    {
      await DisposeTransactionAsync();
    }
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return await base.SaveChangesAsync(cancellationToken);
  }

  private async Task DisposeTransactionAsync()
  {
    if (_currentTransaction != null)
    {
      await _currentTransaction.DisposeAsync();
      _currentTransaction = null;
    }
  }

  public override async ValueTask DisposeAsync()
  {
    if (_currentTransaction != null)
      await _currentTransaction.DisposeAsync();

    await base.DisposeAsync();
  }


  public override void Dispose()
  {
    _currentTransaction?.Dispose();
    base.Dispose();
  }


}
