using Ticketing.Ticket.Domain.Aggregates;
using Ticketing.Ticket.Domain.Entities;
using TicketType = Ticketing.Ticket.Domain.Aggregates.Ticket;

namespace Ticketing.Ticket.TestCommon.Builders
{
  public class TicketBuilder
  {
    private string _subject = "Sample subject";
    private string _description = "Sample description";
    private Guid _userId = Guid.NewGuid();
    private readonly List<TicketReply> _replies = new();


    public TicketBuilder WithSubject(string subject)
    {
      _subject = subject;
      return this;
    }

    public TicketBuilder WithDescription(string description)
    {
      _description = description;
      return this;
    }

    public TicketBuilder WithUser(Guid userId)
    {
      _userId = userId;
      return this;
    }

    public TicketBuilder WithReply(TicketReply reply)
    {
      _replies.Add(reply);
      return this;
    }

    public TicketBuilder WithReplies(IEnumerable<TicketReply> replies)
    {
      _replies.AddRange(replies);
      return this;
    }

    public TicketType Build()
    {
      var ticket = new TicketType(_subject, _description, _userId);

      foreach (var reply in _replies)
      {
        ticket.AddReply(reply);
      }

      return ticket;
    }
  }
}
