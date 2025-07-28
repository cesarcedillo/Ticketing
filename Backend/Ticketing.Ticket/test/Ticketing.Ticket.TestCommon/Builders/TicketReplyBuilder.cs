using Ticketing.Ticket.Domain.Aggregates;
using Ticketing.Ticket.Domain.Entities;
using TicketType = Ticketing.Ticket.Domain.Aggregates.Ticket;

namespace Ticketing.Ticket.TestCommon.Builders;

public class TicketReplyBuilder
{
  private string _text = "Sample reply";
  private Guid _userId = Guid.NewGuid();
  private TicketType _ticket = null!;

  public TicketReplyBuilder WithText(string text)
  {
    _text = text;
    return this;
  }

  public TicketReplyBuilder WithUser(Guid userId)
  {
    _userId = userId;
    return this;
  }

  public TicketReplyBuilder WithTicket(TicketType ticket)
  {
    _ticket = ticket;
    return this;
  }

  public TicketReply Build()
  {
    if (_ticket == null)
      throw new InvalidOperationException("Ticket must be provided to create a TicketReply.");

    return new TicketReply(_text, _userId, _ticket);
  }
}

