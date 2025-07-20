using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Entities;

namespace Ticketing.TestCommon.Builders
{
  public class TicketBuilder
  {
    private string _subject = "Sample subject";
    private string _description = "Sample description";
    private User _user = new UserBuilder().Build();
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

    public TicketBuilder WithUser(User user)
    {
      _user = user;
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

    public Ticket Build()
    {
      var ticket = new Ticket(_subject, _description, _user);

      foreach (var reply in _replies)
      {
        ticket.AddReply(reply);
      }

      return ticket;
    }
  }
}
