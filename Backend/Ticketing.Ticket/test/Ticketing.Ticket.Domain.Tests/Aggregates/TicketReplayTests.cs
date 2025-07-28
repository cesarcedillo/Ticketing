using FluentAssertions;
using Ticketing.Ticket.Domain.Enums;
using Ticketing.Ticket.TestCommon.Builders;

namespace Ticketing.Ticket.Domain.Tests.Aggregates;

public class TicketReplyTests
{
  [Fact]
  public void Create_Reply_Assigns_Properties_Correctly()
  {
    var userId = Guid.NewGuid();
    var anotherUserId = Guid.NewGuid();
    var ticket = new TicketBuilder().WithUser(userId).Build();
    var text = "Test reply";

    var reply = new TicketReplyBuilder()
        .WithText(text)
        .WithUser(anotherUserId)
        .WithTicket(ticket)
        .Build();

    reply.Text.Should().Be(text);
    reply.UserId.Should().Be(anotherUserId);
    reply.Ticket.Should().Be(ticket);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void Cannot_Create_Reply_With_Empty_Text(string text)
  {
    var ticket = new TicketBuilder().Build();

    Action act = () => new TicketReplyBuilder()
        .WithText(text!)
        .WithTicket(ticket)
        .Build();

    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Cannot_Create_Reply_With_Null_User()
  {
    var ticket = new TicketBuilder().Build();

    Action act = () => new TicketReplyBuilder()
        .WithUser(Guid.Empty)
        .WithTicket(ticket)
        .Build();

    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Cannot_Create_Reply_With_Null_Ticket()
  {

    Action act = () => new TicketReplyBuilder()
        .WithTicket(null!)
        .Build();

    act.Should().Throw<InvalidOperationException>();
  }

  [Fact]
  public void CreatedAt_Must_Be_Now_Or_Later_Than_TicketCreation()
  {
    var ticket = new TicketBuilder().Build();

    var reply = new TicketReplyBuilder()
        .WithTicket(ticket)
        .Build();

    reply.CreatedAt.Should().BeOnOrAfter(ticket.CreatedAt);
  }
}
