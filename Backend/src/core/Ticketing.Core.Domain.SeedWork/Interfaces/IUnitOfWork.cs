﻿namespace Ticketing.Core.Domain.SeedWork.Interfaces;
public interface IUnitOfWork : IDisposable
{
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
  void ClearChangeTracker();
}
