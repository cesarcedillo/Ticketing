using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Entities;

namespace Ticketing.TestCommon.Builders;

public class TicketReplyBuilder
{
  private string _text = "Sample reply";
  private User _user = new UserBuilder().Build();
  private Ticket _ticket = null!;

  public TicketReplyBuilder WithText(string text)
  {
    _text = text;
    return this;
  }

  public TicketReplyBuilder WithUser(User user)
  {
    _user = user;
    return this;
  }

  public TicketReplyBuilder WithTicket(Ticket ticket)
  {
    _ticket = ticket;
    return this;
  }

  public TicketReply Build()
  {
    if (_ticket == null)
      throw new InvalidOperationException("Ticket must be provided to create a TicketReply.");

    return new TicketReply(_text, _user, _ticket);
  }
}

