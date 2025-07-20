using FluentAssertions;
using Ticketing.Ticket.Domain.Enums;
using Ticketing.Ticket.TestCommon.Builders;

namespace Ticketing.Ticket.Domain.Tests.Aggregates;

public class TicketReplyTests
{
  [Fact]
  public void Create_Reply_Assigns_Properties_Correctly()
  {
    var user = new UserBuilder().WithUserType(UserType.Agent).Build();
    var ticket = new TicketBuilder().WithUser(new UserBuilder().Build()).Build();
    var text = "Test reply";

    var reply = new TicketReplyBuilder()
        .WithText(text)
        .WithUser(user)
        .WithTicket(ticket)
        .Build();

    reply.Text.Should().Be(text);
    reply.User.Should().Be(user);
    reply.Ticket.Should().Be(ticket);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void Cannot_Create_Reply_With_Empty_Text(string text)
  {
    var user = new UserBuilder().Build();
    var ticket = new TicketBuilder().WithUser(user).Build();

    Action act = () => new TicketReplyBuilder()
        .WithText(text!)
        .WithUser(user)
        .WithTicket(ticket)
        .Build();

    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Cannot_Create_Reply_With_Null_User()
  {
    var ticket = new TicketBuilder().Build();

    Action act = () => new TicketReplyBuilder()
        .WithUser(null!)
        .WithTicket(ticket)
        .Build();

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void Cannot_Create_Reply_With_Null_Ticket()
  {
    var user = new UserBuilder().Build();

    Action act = () => new TicketReplyBuilder()
        .WithUser(user)
        .WithTicket(null!)
        .Build();

    act.Should().Throw<InvalidOperationException>();
  }

  [Fact]
  public void CreatedAt_Must_Be_Now_Or_Later_Than_TicketCreation()
  {
    var user = new UserBuilder().Build();
    var ticket = new TicketBuilder().WithUser(user).Build();

    var reply = new TicketReplyBuilder()
        .WithUser(user)
        .WithTicket(ticket)
        .Build();

    reply.CreatedAt.Should().BeOnOrAfter(ticket.CreatedAt);
  }
}
