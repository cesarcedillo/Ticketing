using MediatR;
using Ticketing.Core.Domain.SeedWork.Abstractions;
using Ticketing.Core.Infrstructure.EntityFramework.Context;

namespace Ticketing.Core.Infrstructure.EntityFramework.Extensions;
public static class MediatorDispatcherExtension
{
  public static async Task DispatchAllDomainEventAsync(this IMediator mediator, DbContextBase dbContextBase)
  {
    var domainEntities = dbContextBase.ChangeTracker
        .Entries<Entity>()
        .Where(a => a.Entity.DomainEvents != null && a.Entity.DomainEvents.Any());

    var domainEvents = domainEntities
        .SelectMany(x => x.Entity.DomainEvents!)
        .ToList();

    domainEntities.ToList()
        .ForEach(entity => entity.Entity.ClearDomainEvents());

    foreach (var domainEvent in domainEvents)
      await mediator.Publish(domainEvent);
  }
}
