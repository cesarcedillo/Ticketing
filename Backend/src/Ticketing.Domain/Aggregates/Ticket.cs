using Ticketing.Core.Domain.SeedWork.Interfaces;

namespace Ticketing.Domain.Aggregates;

public class Ticket : IAggregateRoot
{
  public Guid TicketId { get; private set; }

  public Ticket() { }

  private Ticket(Guid ticketId)
  {
    TicketId = ticketId;
  }

  public static Ticket Crear(Guid ticketId)
  {
    return new Ticket(ticketId);
  }
}

